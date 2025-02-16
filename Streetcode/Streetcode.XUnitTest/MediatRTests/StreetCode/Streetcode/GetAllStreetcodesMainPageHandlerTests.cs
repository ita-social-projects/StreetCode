using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.GetAllMainPage;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Enums;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Streetcode;

public class GetAllStreetcodesMainPageHandlerTests
{
    private readonly Mock<IRepositoryWrapper> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ILoggerService> _loggerMock;
    private readonly MockNoSharedResourceLocalizer _mockLocalizer;
    private readonly GetAllStreetcodesMainPageHandler _handler;
    private List<StreetcodeContent> _testStreetcodes;

    public GetAllStreetcodesMainPageHandlerTests()
    {
        _testStreetcodes = new List<StreetcodeContent>();
        _repositoryMock = new Mock<IRepositoryWrapper>();
        _mapperMock = new Mock<IMapper>();
        _loggerMock = new Mock<ILoggerService>();
        _mockLocalizer = new MockNoSharedResourceLocalizer();

        _handler = new GetAllStreetcodesMainPageHandler(
            _repositoryMock.Object,
            _mapperMock.Object,
            _loggerMock.Object,
            _mockLocalizer);

        SetupMocks(5);
    }

    [Fact]
    public async Task Handle_WhenStreetcodesExist_ReturnsShuffledMainPageDTOs()
    {
        // Arrange
        var request = new GetAllStreetcodesMainPageQuery();

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.True(result.IsSuccess);
            Assert.Equal(_testStreetcodes.Count, result.Value.Count());
            _repositoryMock.Verify(
                repo => repo.StreetcodeRepository.GetAllAsync(
                    It.IsAny<Expression<Func<StreetcodeContent, bool>>>(),
                    It.IsAny<Func<IQueryable<StreetcodeContent>,
                        IIncludableQueryable<StreetcodeContent, object>>>()), Times.Once);
            _mapperMock.Verify(
                m => m.Map<IEnumerable<StreetcodeMainPageDTO>>(It.IsAny<IEnumerable<StreetcodeContent>>()), Times.Once);
        });
    }

    [Fact]
    public async Task Handle_WhenNoStreetcodesExist_ReturnsError()
    {
        // Arrange
        var request = new GetAllStreetcodesMainPageQuery();
        const string expectedErrorKey = "NoStreetcodesExistNow";
        string expectedErrorValue = _mockLocalizer[expectedErrorKey];

        _repositoryMock
            .Setup(repo => repo.StreetcodeRepository.GetAllAsync(
                It.IsAny<Expression<Func<StreetcodeContent, bool>>>(),
                It.IsAny<Func<IQueryable<StreetcodeContent>, IIncludableQueryable<StreetcodeContent, object>>>()))
            .ReturnsAsync(new List<StreetcodeContent>());

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.False(result.IsSuccess);
            Assert.Contains(expectedErrorValue, result.Errors.Single().Message);
            _loggerMock.Verify(logger => logger.LogError(request, expectedErrorValue), Times.Once);
        });
    }

    [Fact]
    public async Task Handle_FiltersImagesCorrectly()
    {
        // Arrange
        var request = new GetAllStreetcodesMainPageQuery();

        // Act
        await _handler.Handle(request, CancellationToken.None);

        // Assert
        foreach (var streetcode in _testStreetcodes)
        {
            Assert.All(streetcode.Images, image =>
            {
                Assert.NotNull(image.ImageDetails);
                Assert.Equal(((int)ImageAssigment.Blackandwhite).ToString(), image.ImageDetails.Alt);
            });
        }
    }

    private void SetupMocks(int count)
    {
        _testStreetcodes = Enumerable.Range(1, count).Select(i => new StreetcodeContent
        {
            Id = i,
            Status = StreetcodeStatus.Published,
            Images = new List<Image>
            {
                new () { ImageDetails = new ImageDetails { Alt = "1" } },
                new () { ImageDetails = new ImageDetails { Alt = "WrongValue" } },
            },
        }).ToList();

        _repositoryMock
            .Setup(repo => repo.StreetcodeRepository.GetAllAsync(
                It.IsAny<Expression<Func<StreetcodeContent, bool>>>(),
                It.IsAny<Func<IQueryable<StreetcodeContent>, IIncludableQueryable<StreetcodeContent, object>>>()))
            .ReturnsAsync(_testStreetcodes);

        _mapperMock
            .Setup(m => m.Map<IEnumerable<StreetcodeMainPageDTO>>(It.IsAny<IEnumerable<StreetcodeContent>>()))
            .Returns((IEnumerable<StreetcodeContent> src) =>
                src.Select(s => new StreetcodeMainPageDTO { Id = s.Id }).ToList());
    }
}