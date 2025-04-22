using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.GetPageMainPage;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Enums;
using Streetcode.DAL.Helpers;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Streetcode;

public class GetPageOfStreetcodesMainPageHandlerTests
{
    private readonly Mock<IRepositoryWrapper> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ILoggerService> _loggerMock;
    private readonly MockNoSharedResourceLocalizer _mockLocalizer;
    private readonly GetPageOfStreetcodesMainPageHandler _handler;

    public GetPageOfStreetcodesMainPageHandlerTests()
    {
        _repositoryMock = new Mock<IRepositoryWrapper>();
        _mapperMock = new Mock<IMapper>();
        _loggerMock = new Mock<ILoggerService>();
        _mockLocalizer = new MockNoSharedResourceLocalizer();

        _handler = new GetPageOfStreetcodesMainPageHandler(
            _repositoryMock.Object,
            _mapperMock.Object,
            _loggerMock.Object,
            _mockLocalizer);
    }

    [Fact]
    public async Task Handle_WhenStreetcodesExist_ReturnsPagedCatalogItems()
    {
        // Arrange
        var request = new GetPageOfStreetcodesMainPageQuery(Page: 1, PageSize: 2);
        var testStreetcodes = GetTestStreetcodes(5);

        SetupRepositoryMocks(testStreetcodes, request.Page, request.PageSize);

        _mapperMock
            .Setup(m => m.Map<IEnumerable<StreetcodeMainPageDTO>>(It.IsAny<IEnumerable<StreetcodeContent>>()))
            .Returns((IEnumerable<StreetcodeContent> src) => src.Select(s => new StreetcodeMainPageDTO { Id = s.Id }).ToList());

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.True(result.IsSuccess);
            Assert.Equal(request.PageSize, result.Value.Count());
        });
    }

    [Fact]
    public async Task Handle_WhenNoStreetcodesExist_ReturnsError()
    {
        // Arrange
        var request = new GetPageOfStreetcodesMainPageQuery(Page: 1, PageSize: 2);
        string expectedErrorMessage = _mockLocalizer["NoStreetcodesExistNow"];

        SetupRepositoryMocks(new List<StreetcodeContent>());

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.False(result.IsSuccess);
            Assert.Contains(expectedErrorMessage, result.Errors.Single().Message);
            _loggerMock.Verify(logger => logger.LogError(request, expectedErrorMessage), Times.Once);
        });
    }

    [Fact]
    public async Task Handle_WhenStreetcodesExist_ShufflesResults()
    {
        // Arrange
        var request = new GetPageOfStreetcodesMainPageQuery(Page: 1, PageSize: 5);
        var testStreetcodes = GetTestStreetcodes(10);

        SetupRepositoryMocks(testStreetcodes);

        // Act
        var result1 = await _handler.Handle(request, CancellationToken.None);
        var result2 = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.True(result1.IsSuccess);
            Assert.True(result2.IsSuccess);
            Assert.NotEqual(result1.Value, result2.Value);
        });
    }

    private static List<StreetcodeContent> GetTestStreetcodes(int count)
    {
        return Enumerable.Range(1, count).Select(i => new StreetcodeContent
        {
            Id = i,
            Status = StreetcodeStatus.Published,
            CreatedAt = DateTime.UtcNow.AddDays(-i),
            Text = new Text(),
            Images = new List<Image>
            {
                new Image()
                {
                    ImageDetails = new ImageDetails { Alt = ((int)ImageAssigment.Blackandwhite).ToString() },
                },
            },
        }).ToList();
    }

    private void SetupRepositoryMocks(List<StreetcodeContent> streetcodes, ushort pageNumber = 1, ushort pageSize = 5)
    {
        var paginationResponse = PaginationResponse<StreetcodeContent>.Create(streetcodes.AsQueryable(), pageNumber, pageSize);

        _repositoryMock
            .Setup(repo => repo.StreetcodeRepository.GetAllPaginated(
                It.IsAny<ushort>(),
                It.IsAny<ushort>(),
                It.IsAny<Expression<Func<StreetcodeContent, StreetcodeContent>>>(),
                It.IsAny<Expression<Func<StreetcodeContent, bool>>>(),
                It.IsAny<Func<IQueryable<StreetcodeContent>, IIncludableQueryable<StreetcodeContent, object>>>(),
                It.IsAny<Expression<Func<StreetcodeContent, object>>>(),
                It.IsAny<Expression<Func<StreetcodeContent, object>>>()))
            .Returns(paginationResponse);

        _mapperMock
            .Setup(m => m.Map<IEnumerable<StreetcodeMainPageDTO>>(It.IsAny<IEnumerable<StreetcodeContent>>()))
            .Returns((IEnumerable<StreetcodeContent> src) => src.Select(s => new StreetcodeMainPageDTO { Id = s.Id }).ToList());
    }
}