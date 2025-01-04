using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Timeline;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Timeline.HistoricalContext.GetByTitle;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Timeline;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Timeline.HistoricalContextTests;

public class GetByTitleHistoricalContextTest
{
    private const string title = "test_title";
    private readonly Mock<IRepositoryWrapper> mockRepo;
    private readonly Mock<IMapper> mockMapper;
    private readonly Mock<ILoggerService> mockLogger;
    private readonly Mock<IStringLocalizer<CannotFindSharedResource>> mockLocalizer;

    private readonly HistoricalContext context = new HistoricalContext
    {
        Id = 1,
        Title = title,
    };

    private readonly HistoricalContextDTO contextDto = new HistoricalContextDTO
    {
        Id = 1,
        Title = title,
    };

    public GetByTitleHistoricalContextTest()
    {
        this.mockRepo = new Mock<IRepositoryWrapper>();
        this.mockMapper = new Mock<IMapper>();
        this.mockLogger = new Mock<ILoggerService>();
        this.mockLocalizer = new Mock<IStringLocalizer<CannotFindSharedResource>>();
    }

    [Fact]
    public async Task Handler_Returns_Matching_Element()
    {
        // Arrange
        this.SetupRepository(this.context);
        this.SetupMapper(this.contextDto);

        var handler = new GetHistoricalContextByTitleHandler(this.mockMapper.Object, this.mockRepo.Object, this.mockLogger.Object, this.mockLocalizer.Object);

        // Act
        var result = await handler.Handle(new GetHistoricalContextByTitleQuery(title), CancellationToken.None);

        // Assert
        Assert.Multiple(
            () => Assert.IsType<HistoricalContextDTO>(result.Value),
            () => Assert.Equal(title, result.Value.Title));
    }

    [Fact]
    public async Task Handler_Returns_NoMatching_Element()
    {
        // Arrange
        this.SetupRepository(new HistoricalContext());
        this.SetupMapper(new HistoricalContextDTO());

        var handler = new GetHistoricalContextByTitleHandler(this.mockMapper.Object, this.mockRepo.Object, this.mockLogger.Object, this.mockLocalizer.Object);

        // Act
        var result = await handler.Handle(new GetHistoricalContextByTitleQuery(title), CancellationToken.None);

        // Assert
        Assert.Multiple(
            () => Assert.IsType<HistoricalContextDTO>(result.Value),
            () => Assert.Null(result.Value.Title));
    }

    private void SetupRepository(HistoricalContext context)
    {
        this.mockRepo
            .Setup(repo => repo.HistoricalContextRepository
                .GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<HistoricalContext, bool>>>(),
                    It.IsAny<Func<IQueryable<HistoricalContext>,
                    IIncludableQueryable<HistoricalContext, object>>>()))
            .ReturnsAsync(context);
    }

    private void SetupMapper(HistoricalContextDTO contextDto)
    {
        this.mockMapper.Setup(x => x.Map<HistoricalContextDTO>(It.IsAny<HistoricalContext>()))
            .Returns(contextDto);
    }
}