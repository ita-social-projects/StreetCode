using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Streetcode.CatalogItem;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.GetAllCatalog;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Enums;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Streetcode;

public class GetAllStreetcodesCatalogHandlerTests
{
    private readonly Mock<IRepositoryWrapper> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ILoggerService> _mockLogger;
    private readonly MockNoSharedResourceLocalizer _mockNoStreetcodesLocalizer;
    private readonly GetAllStreetcodesCatalogHandler _handler;

    public GetAllStreetcodesCatalogHandlerTests()
    {
        _repositoryMock = new Mock<IRepositoryWrapper>();
        _mapperMock = new Mock<IMapper>();
        _mockLogger = new Mock<ILoggerService>();
        _mockNoStreetcodesLocalizer = new MockNoSharedResourceLocalizer();

        _handler = new GetAllStreetcodesCatalogHandler(
            _repositoryMock.Object,
            _mapperMock.Object,
            _mockLogger.Object,
            _mockNoStreetcodesLocalizer);
    }

    [Fact]
    public async Task Handle_WhenStreetcodesExist_ReturnsPagedCatalogItems()
    {
        // Arrange
        var request = new GetAllStreetcodesCatalogQuery(Page: 1, Count: 2);
        var testStreetcodes = GetTestStreetcodes(5);

        SetupMocksForStreetcodeCatalogHandler(testStreetcodes);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.True(result.IsSuccess);
            Assert.Equal(request.Count, result.Value.Count());
            Assert.Equal(1, result.Value.ElementAt(0).Id);
            Assert.Equal(2, result.Value.ElementAt(1).Id);
            _repositoryMock.Verify(
                repo => repo.StreetcodeRepository.GetAllAsync(
                    It.IsAny<Expression<Func<StreetcodeContent, bool>>>(),
                    It.IsAny<Func<IQueryable<StreetcodeContent>, IIncludableQueryable<StreetcodeContent, object>>>()), Times.Once);
            _mapperMock.Verify(m => m.Map<IEnumerable<CatalogItem>>(testStreetcodes), Times.Once);
        });
    }

    [Fact]
    public async Task Handle_WhenNoStreetcodesExist_ReturnsError()
    {
        // Arrange
        var request = new GetAllStreetcodesCatalogQuery(Page: 1, Count: 2);
        string expectedErrorKey = "NoStreetcodesExistNow";
        string expectedErrorValue = _mockNoStreetcodesLocalizer[expectedErrorKey];

        SetupMocksForStreetcodeCatalogHandler(new List<StreetcodeContent>());

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.False(result.IsSuccess);
            Assert.Contains(expectedErrorValue, result.Errors.Single().Message);
            _mockLogger.Verify(logger => logger.LogError(request, expectedErrorValue), Times.Once);
        });
    }

    [Fact]
    public async Task Handle_WhenStreetcodesExist_FiltersImagesByKeyNumOfImageToDisplay()
    {
        // Arrange
        var request = new GetAllStreetcodesCatalogQuery(Page: 1, Count: 2);
        const int keyNumOfImageToDisplay = (int)ImageAssigment.Blackandwhite;

        var testStreetcodes = new List<StreetcodeContent>
        {
            new ()
            {
                Id = 1,
                Images = new List<Image>
                {
                    new () { ImageDetails = new ImageDetails { Alt = keyNumOfImageToDisplay.ToString() } },
                    new () { ImageDetails = new ImageDetails { Alt = "WrongValue" } },
                },
            },
            new ()
            {
                Id = 2,
                Images = new List<Image>
                {
                    new () { ImageDetails = new ImageDetails { Alt = keyNumOfImageToDisplay.ToString() } },
                    new () { ImageDetails = null },
                },
            },
        };

        SetupMocksForStreetcodeCatalogHandler(testStreetcodes);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(request.Count, result.Value.Count());

        foreach (var streetcode in testStreetcodes)
        {
            Assert.All(streetcode.Images, image =>
            {
                Assert.NotNull(image.ImageDetails);
                if (image.ImageDetails is not null)
                {
                    Assert.Equal(keyNumOfImageToDisplay.ToString(), image.ImageDetails.Alt);
                }
            });
        }
    }

    private static List<StreetcodeContent> GetTestStreetcodes(int count)
    {
        var streetcodes = new List<StreetcodeContent>();
        for (int i = 1; i <= count; i++)
        {
            streetcodes.Add(new StreetcodeContent
            {
                Id = i,
                Status = StreetcodeStatus.Published,
                Images = new List<Image>
                {
                    new ()
                    {
                        ImageDetails = new ImageDetails { Alt = ((int)ImageAssigment.Blackandwhite).ToString() },
                    },
                },
            });
        }

        return streetcodes;
    }

    private void SetupMocksForStreetcodeCatalogHandler(List<StreetcodeContent> streetcodes)
    {
        _repositoryMock
            .Setup(repo => repo.StreetcodeRepository.GetAllAsync(
                It.IsAny<Expression<Func<StreetcodeContent, bool>>>(),
                It.IsAny<Func<IQueryable<StreetcodeContent>, IIncludableQueryable<StreetcodeContent, object>>>()))
            .ReturnsAsync(streetcodes);

        _mapperMock
            .Setup(m => m.Map<IEnumerable<CatalogItem>>(It.IsAny<IEnumerable<StreetcodeContent>>()))
            .Returns((IEnumerable<StreetcodeContent> src) =>
                src.Select(s => new CatalogItem()
                {
                    Id = s.Id,
                }).ToList());
    }
}
