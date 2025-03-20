using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using MockQueryable.Moq;
using Moq;
using Streetcode.BLL.DTO.Streetcode.RelatedFigure;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.RelatedFigure.GetByStreetcodeId;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Enums;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.XUnitTest.Mocks;
using Xunit;
using Entities = Streetcode.DAL.Entities.Streetcode;

namespace Streetcode.XUnitTest.MediatRTests.Streetcode.RelatedFigure;

public class GetRelatedFiguresByStreetcodeIdHandlerTests
{
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IRepositoryWrapper> _repositoryMock;
    private readonly Mock<ILoggerService> _loggerMock;
    private readonly MockCannotFindLocalizer _mockLocalizerCannotFind;
    private readonly GetRelatedFiguresByStreetcodeIdHandler _handler;

    public GetRelatedFiguresByStreetcodeIdHandlerTests()
    {
        _mapperMock = new Mock<IMapper>();
        _repositoryMock = new Mock<IRepositoryWrapper>();
        _loggerMock = new Mock<ILoggerService>();
        _mockLocalizerCannotFind = new MockCannotFindLocalizer();
        _handler = new GetRelatedFiguresByStreetcodeIdHandler(
            _mapperMock.Object,
            _repositoryMock.Object,
            _loggerMock.Object,
            _mockLocalizerCannotFind);
    }

    [Fact]
    public async Task Handle_WhenRelatedFiguresExist_ReturnsMappedResults()
    {
        // Arrange
        var relatedFigureIds = new List<int> { 1, 2 };
        var streetcodes = new List<StreetcodeContent>
        {
            new () { Id = 1, Status = StreetcodeStatus.Published },
            new () { Id = 2, Status = StreetcodeStatus.Published },
        };

        SetupMocksForRelatedFigures(relatedFigureIds, streetcodes, streetcodes);

        _mapperMock
            .Setup(m => m.Map<IEnumerable<RelatedFigureDTO>>(It.IsAny<IEnumerable<StreetcodeContent>>()))
            .Returns((IEnumerable<StreetcodeContent> src) => src.Select(s => new RelatedFigureDTO { Id = s.Id }));

        var request = new GetRelatedFigureByStreetcodeIdQuery(1, UserRole.User);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(2, result.Value.Count());
            Assert.Contains(result.Value, dto => dto.Id == 1);
            Assert.Contains(result.Value, dto => dto.Id == 2);
        });
    }

    [Fact]
    public async Task Handle_WhenNoRelatedFiguresExist_ReturnsEmptyResult()
    {
        // Arrange
        var streetcodesUserHaveAccessTo = new List<StreetcodeContent>
        {
            new () { Id = 1, Status = StreetcodeStatus.Draft },
        };

        SetupMocksForRelatedFigures(new List<int>(), new List<StreetcodeContent>(), streetcodesUserHaveAccessTo);
        var request = new GetRelatedFigureByStreetcodeIdQuery(1, UserRole.User);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Empty(result.Value);
            _loggerMock.Verify(logger => logger.LogInformation("Returning empty enumerable of related figures"), Times.Once);
        });
    }

    [Fact]
    public async Task Handle_WhenUserDoesNotHaveAccessToStreetcode_ReturnsError()
    {
        // Arrange
        var relatedFigureIds = new List<int> { 1, 2 };
        var streetcodes = new List<StreetcodeContent>
        {
            new () { Id = 1, Status = StreetcodeStatus.Draft },
            new () { Id = 2, Status = StreetcodeStatus.Deleted },
        };

        SetupMocksForRelatedFigures(relatedFigureIds, streetcodes, new List<StreetcodeContent>());
        var expectedError = _mockLocalizerCannotFind["CannotFindAnAudioWithTheCorrespondingStreetcodeId", 1].Value;

        var request = new GetRelatedFigureByStreetcodeIdQuery(1, UserRole.User);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(expectedError, result.Errors.Single().Message);
    }

    [Fact]
    public async Task Handle_WhenExceptionOccurs_ReturnsEmptyResult()
    {
        // Arrange
        var streetcodesUserHaveAccessTo = new List<StreetcodeContent>
        {
            new () { Id = 1, Status = StreetcodeStatus.Draft },
        };

        _repositoryMock
            .Setup(repo => repo.StreetcodeRepository.FindAll(
                It.IsAny<Expression<Func<StreetcodeContent, bool>>>(),
                It.IsAny<Func<IQueryable<StreetcodeContent>,
                    IIncludableQueryable<StreetcodeContent, object>>>()))
            .Returns(streetcodesUserHaveAccessTo.AsQueryable().BuildMockDbSet().Object);

        _repositoryMock
            .Setup(repo => repo.RelatedFigureRepository.FindAll(It.IsAny<Expression<Func<Entities.RelatedFigure, bool>>>(), null))
            .Throws(new ArgumentNullException());

        var request = new GetRelatedFigureByStreetcodeIdQuery(1, UserRole.User);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.True(result.IsSuccess);
            Assert.Empty(result.Value!);
        });
    }

    [Fact]
    public async Task Handle_WhenStreetcodesHaveImages_OrdersImagesByAlt()
    {
        // Arrange
        var relatedFigureIds = new List<int> { 1 };
        var streetcodes = new List<StreetcodeContent>
        {
            new ()
            {
                Id = 1,
                Status = StreetcodeStatus.Published,
                Images = new List<Image>
                {
                    new () { ImageDetails = new ImageDetails { Alt = "0" } },
                    new () { ImageDetails = new ImageDetails { Alt = "1" } },
                },
            },
        };

        SetupMocksForRelatedFigures(relatedFigureIds, streetcodes, streetcodes);

        _mapperMock
            .Setup(m => m.Map<IEnumerable<RelatedFigureDTO>>(It.IsAny<IEnumerable<StreetcodeContent>>()))
            .Returns((
                IEnumerable<StreetcodeContent> src) => src.Select(s => new RelatedFigureDTO { Id = s.Id }));

        var request = new GetRelatedFigureByStreetcodeIdQuery(1, UserRole.User);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(1, result.Value.First().Id);
            Assert.Equal("0", streetcodes.First().Images[0].ImageDetails?.Alt);
            Assert.Equal("1", streetcodes.First().Images[1].ImageDetails?.Alt);
        });
    }

    private void SetupMocksForRelatedFigures(
        IEnumerable<int> relatedFigureIds,
        IEnumerable<StreetcodeContent> streetcodes,
        List<StreetcodeContent> streetcodeListUserCanAccess)
    {
        _repositoryMock
            .Setup(repo => repo.RelatedFigureRepository.FindAll(
                It.IsAny<Expression<Func<Entities.RelatedFigure, bool>>>(),
                It.IsAny<Func<IQueryable<Entities.RelatedFigure>,
                    IIncludableQueryable<Entities.RelatedFigure, object>>>()))
            .Returns((
                Expression<Func<Entities.RelatedFigure, bool>> _,
                Func<IQueryable<Entities.RelatedFigure>, IIncludableQueryable<Entities.RelatedFigure, object>>
                    include) =>
            {
                var queryableData = relatedFigureIds
                    .Select(id => new Entities.RelatedFigure { ObserverId = id, TargetId = id })
                    .AsQueryable();

                return include != null ? include(queryableData) : queryableData;
            });

        _repositoryMock
            .Setup(repo => repo.StreetcodeRepository.GetAllAsync(
                It.IsAny<Expression<Func<StreetcodeContent, bool>>>(),
                It.IsAny<Func<IQueryable<StreetcodeContent>, IIncludableQueryable<StreetcodeContent, object>>>()))
            .ReturnsAsync(streetcodes);

        _repositoryMock
            .Setup(repo => repo.StreetcodeRepository.FindAll(
                    It.IsAny<Expression<Func<StreetcodeContent, bool>>>(),
                    It.IsAny<Func<IQueryable<StreetcodeContent>,
                        IIncludableQueryable<StreetcodeContent, object>>>()))
            .Returns(streetcodeListUserCanAccess.AsQueryable().BuildMockDbSet().Object);
    }
}