using System.Linq.Expressions;
using FluentAssertions;
using Moq;
using Streetcode.BLL.DTO.AdditionalContent.Filter;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.GetByFilter;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Entities.Timeline;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Streetcode;

public class GetStreetcodeByFilterHandlerTests
{
    private readonly Mock<IRepositoryWrapper> _repositoryWrapper;
    private readonly GetStreetcodeByFilterHandler _handler;

    public GetStreetcodeByFilterHandlerTests()
    {
        this._repositoryWrapper = new Mock<IRepositoryWrapper>();
        this._handler = new GetStreetcodeByFilterHandler(this._repositoryWrapper.Object);
    }

    [Theory]
    [InlineData("Test", "Test Streetcode")]
    [InlineData("TextContent", "TextContent")]
    [InlineData("FactTitle", "FactTitle")]
    [InlineData("TimelineTitle", "TimelineTitle")]
    [InlineData("ArtDescription", "ArtDescription")]
    public async Task Handle_ReturnsResults_WhenSearchQueryMatches(string searchQuery, string expectedContent)
    {
        // Arrange
        var query = new GetStreetcodeByFilterQuery(new StreetcodeFilterRequestDTO { SearchQuery = searchQuery });

        this.SetupRepositoryMock(expectedContent);

        // Act
        var result = await this._handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().ContainSingle();
        result.Value.First().Content.Should().Be(expectedContent);
    }

    [Theory]
    [InlineData("NonExistent")]
    public async Task Handle_ReturnsEmptyList_WhenNoMatchesFound(string searchQuery)
    {
        // Arrange
        var query = new GetStreetcodeByFilterQuery(new StreetcodeFilterRequestDTO { SearchQuery = searchQuery });

        this.SetupRepositoryMock(null, true);

        // Act
        var result = await this._handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEmpty();
    }

    [Theory]
    [InlineData("Test")]
    public async Task Handle_ThrowsException_WhenRepositoryThrowsException(string searchQuery)
    {
        // Arrange
        var query = new GetStreetcodeByFilterQuery(new StreetcodeFilterRequestDTO { SearchQuery = searchQuery });

        this._repositoryWrapper.Setup(r => r.StreetcodeRepository
            .GetAllAsync(It.IsAny<Expression<Func<StreetcodeContent, bool>>>(), null))
            .ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        await FluentActions.Invoking(() => this._handler.Handle(query, CancellationToken.None)).Should().ThrowAsync<Exception>();
    }

    private void SetupRepositoryMock(string? expectedContent, bool returnEmptyList = false)
    {
        var streetcodes = returnEmptyList ? new List<StreetcodeContent>() : new List<StreetcodeContent>
        {
            new () { Id = 1, Title = expectedContent, Status = DAL.Enums.StreetcodeStatus.Published },
        };

        var texts = returnEmptyList ? new List<Text>() : new List<Text>
        {
            new () { Streetcode = new StreetcodeContent { Id = 1, Status = DAL.Enums.StreetcodeStatus.Published }, TextContent = expectedContent },
        };

        var streetcodeArts = returnEmptyList ? new List<StreetcodeArt>() : new List<StreetcodeArt>
        {
            new () { Art = new Art { Description = expectedContent }, StreetcodeArtSlide = new StreetcodeArtSlide { Streetcode = new StreetcodeContent { Id = 1, Status = DAL.Enums.StreetcodeStatus.Published } } },
        };

        var facts = returnEmptyList ? new List<Fact>() : new List<Fact>
        {
            new () { Streetcode = new StreetcodeContent { Id = 1, Status = DAL.Enums.StreetcodeStatus.Published }, Title = expectedContent, FactContent = expectedContent },
        };

        var timelineItems = returnEmptyList ? new List<TimelineItem>() : new List<TimelineItem>
        {
            new () { Streetcode = new StreetcodeContent { Id = 1, Status = DAL.Enums.StreetcodeStatus.Published }, Title = expectedContent, Description = expectedContent },
        };

        this._repositoryWrapper.Setup(r => r.StreetcodeRepository
            .GetAllAsync(It.IsAny<Expression<Func<StreetcodeContent, bool>>>(), null))
            .ReturnsAsync(streetcodes);

        this._repositoryWrapper.Setup(r => r.TextRepository
            .GetAllAsync(It.IsAny<Expression<Func<Text, bool>>>(), null))
            .ReturnsAsync(texts);

        this._repositoryWrapper.Setup(r => r.StreetcodeArtRepository
            .GetAllAsync(It.IsAny<Expression<Func<StreetcodeArt, bool>>>(), null))
            .ReturnsAsync(streetcodeArts);

        this._repositoryWrapper.Setup(r => r.FactRepository
            .GetAllAsync(It.IsAny<Expression<Func<Fact, bool>>>(), null))
            .ReturnsAsync(facts);

        this._repositoryWrapper.Setup(r => r.TimelineRepository
            .GetAllAsync(It.IsAny<Expression<Func<TimelineItem, bool>>>(), null))
            .ReturnsAsync(timelineItems);
    }
}