using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Timeline;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Timeline.HistoricalContext.GetByTitle;
using Streetcode.DAL.Entities.Timeline;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Timeline.HistoricalContextTests;

public class GetByTitleHistoricalContextTest
{
    private const string _title = "test_title";
    private readonly Mock<IRepositoryWrapper> _mockRepo;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILoggerService> _mockLogger;

    private readonly HistoricalContext context = new HistoricalContext
    {
        Id = 1,
        Title = _title,
    };

    private readonly HistoricalContextDTO contextDto = new HistoricalContextDTO
    {
        Id = 1,
        Title = _title,
    };

    public GetByTitleHistoricalContextTest()
    {
        this._mockRepo = new Mock<IRepositoryWrapper>();
        this._mockMapper = new Mock<IMapper>();
        this._mockLogger = new Mock<ILoggerService>();
    }

    [Fact]
    public async Task Handler_Returns_Matching_Element()
    {
        // Arrange
        this.SetupRepository(this.context);
        this.SetupMapper(this.contextDto);

        var handler = new GetHistoricalContextByTitleHandler(this._mockMapper.Object, this._mockRepo.Object, this._mockLogger.Object);

        // Act
        var result = await handler.Handle(new GetHistoricalContextByTitleQuery(_title), CancellationToken.None);

        // Assert
        Assert.Multiple(
            () => Assert.IsType<HistoricalContextDTO>(result.Value),
            () => Assert.Equal(_title, result.Value.Title));
    }

    [Fact]
    public async Task Handler_Returns_NoMatching_Element()
    {
        // Arrange
        this.SetupRepository(new HistoricalContext());
        this.SetupMapper(new HistoricalContextDTO());

        var handler = new GetHistoricalContextByTitleHandler(this._mockMapper.Object, this._mockRepo.Object, this._mockLogger.Object);

        // Act
        var result = await handler.Handle(new GetHistoricalContextByTitleQuery(_title), CancellationToken.None);

        // Assert
        Assert.Multiple(
            () => Assert.IsType<HistoricalContextDTO>(result.Value),
            () => Assert.Null(result.Value.Title));
    }

    private void SetupRepository(HistoricalContext context)
    {
        this._mockRepo
            .Setup(repo => repo.HistoricalContextRepository
                .GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<HistoricalContext, bool>>>(),
                    It.IsAny<Func<IQueryable<HistoricalContext>,
                    IIncludableQueryable<HistoricalContext, object>>>()))
            .ReturnsAsync(context);
    }

    private void SetupMapper(HistoricalContextDTO contextDto)
    {
        this._mockMapper.Setup(x => x.Map<HistoricalContextDTO>(It.IsAny<HistoricalContext>()))
            .Returns(contextDto);
    }
}