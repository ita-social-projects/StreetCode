using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Streetcode.RelatedFigure;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.RelatedFigure.GetByTagId;
using Streetcode.DAL.Entities.AdditionalContent;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Enums;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Streetcode.RelatedFigure;

public class GetRelatedFiguresByTagIdHandlerTests
{
    private readonly Mock<IRepositoryWrapper> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ILoggerService> _loggerMock;
    private readonly GetRelatedFiguresByTagIdHandler _handler;

    public GetRelatedFiguresByTagIdHandlerTests()
    {
        _repositoryMock = new Mock<IRepositoryWrapper>();
        _mapperMock = new Mock<IMapper>();
        _loggerMock = new Mock<ILoggerService>();

        _handler = new GetRelatedFiguresByTagIdHandler(
            _repositoryMock.Object,
            _mapperMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_WhenStreetcodesExist_ReturnsMappedResults()
    {
        // Arrange
        var request = new GetRelatedFiguresByTagIdQuery(1);
        var testStreetcodes = GetTestStreetcodes(3, request.TagId);

        SetupMocksForStreetcodes(testStreetcodes);

        _mapperMock
            .Setup(m => m.Map<IEnumerable<RelatedFigureDTO>>(It.IsAny<IEnumerable<StreetcodeContent>>()))
            .Returns((IEnumerable<StreetcodeContent> src) => src.Select(sc => new RelatedFigureDTO { Id = sc.Id }).ToList());

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.True(result.IsSuccess);
            Assert.NotEmpty(result.Value);
            Assert.Equal(testStreetcodes.Count, result.Value.Count());
        });
    }

    [Fact]
    public async Task Handle_WhenNoStreetcodesExist_ReturnsEmptyResult()
    {
        // Arrange
        var request = new GetRelatedFiguresByTagIdQuery(99);
        SetupMocksForStreetcodes(new List<StreetcodeContent>());

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.True(result.IsSuccess);
            Assert.Empty(result.Value);
            _loggerMock.Verify(logger => logger.LogInformation("Returning empty enumerable of related figures"), Times.Once);
        });
    }

    [Fact]
    public async Task Handle_WhenFilteringImages_OnlyBlackAndWhiteAreKept()
    {
        // Arrange
        var request = new GetRelatedFiguresByTagIdQuery(1);
        var testStreetcodes = GetTestStreetcodes(3, request.TagId);

        SetupMocksForStreetcodes(testStreetcodes);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.All(testStreetcodes, streetcode =>
        {
            Assert.All(streetcode.Images, img =>
            {
                Assert.Equal(((int)ImageAssigment.Blackandwhite).ToString(), img.ImageDetails?.Alt);
            });
        });
    }

    private static List<StreetcodeContent> GetTestStreetcodes(int count, int tagId)
    {
        return Enumerable.Range(1, count).Select(i => new StreetcodeContent
        {
            Id = i,
            Status = StreetcodeStatus.Published,
            Tags = new List<Tag> { new () { Id = tagId } },
            Images = new List<Image>
            {
                new () { ImageDetails = new ImageDetails { Alt = ((int)ImageAssigment.Blackandwhite).ToString() } },
            },
        }).ToList();
    }

    private void SetupMocksForStreetcodes(IEnumerable<StreetcodeContent> streetcodes)
    {
        _repositoryMock
            .Setup(repo => repo.StreetcodeRepository.GetAllAsync(
                It.IsAny<Expression<Func<StreetcodeContent, bool>>>(),
                It.IsAny<Func<IQueryable<StreetcodeContent>, IIncludableQueryable<StreetcodeContent, object>>>()))
            .ReturnsAsync(streetcodes);
    }
}