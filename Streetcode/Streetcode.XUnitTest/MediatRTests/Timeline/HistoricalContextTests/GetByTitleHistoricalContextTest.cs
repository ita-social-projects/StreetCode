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
    private readonly Mock<IRepositoryWrapper> _mockRepo;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILoggerService> _mockLogger;

    public GetByTitleHistoricalContextTest()
    {
        _mockRepo = new Mock<IRepositoryWrapper>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILoggerService>();
    }

    private static string _title = "test_title";

    private readonly HistoricalContext context = new HistoricalContext
    {
        Id = 1,
        Title = _title
    };

    private readonly HistoricalContextDTO contextDto = new HistoricalContextDTO
    {
        Id = 1,
        Title = _title
    };

    async Task SetupRepository(HistoricalContext context)
    {
        _mockRepo.Setup(repo => repo.HistoricalContextRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<HistoricalContext, bool>>>(),
                It.IsAny<Func<IQueryable<HistoricalContext>,
                    IIncludableQueryable<HistoricalContext, object>>>()))
            .ReturnsAsync(context);
    }

    async Task SetupMapper(HistoricalContextDTO contextDto)
    {
        _mockMapper.Setup(x => x.Map<HistoricalContextDTO>(It.IsAny<HistoricalContext>()))
            .Returns(contextDto);
    }

    [Fact]
    public async Task Handler_Returns_Matching_Element()
    {
        //Arrange
        await SetupRepository(context);
        await SetupMapper(contextDto);

        var handler = new GetHistoricalContextByTitleHandler(_mockMapper.Object, _mockRepo.Object, _mockLogger.Object);

        //Act
        var result = await handler.Handle(new GetHistoricalContextByTitleQuery(_title), CancellationToken.None);

        //Assert
        Assert.Multiple(
            () => Assert.IsType<HistoricalContextDTO>(result.Value),
            () => Assert.Equal(result.Value.Title, _title));
    }

    [Fact]
    public async Task Handler_Returns_NoMatching_Element()
    {
        //Arrange
        await SetupRepository(new HistoricalContext());
        await SetupMapper(new HistoricalContextDTO());

        var handler = new GetHistoricalContextByTitleHandler(_mockMapper.Object, _mockRepo.Object, _mockLogger.Object);

        //Act
        var result = await handler.Handle(new GetHistoricalContextByTitleQuery(_title), CancellationToken.None);

        //Assert
        Assert.Multiple(
            () => Assert.IsType<HistoricalContextDTO>(result.Value),
            () => Assert.Null(result.Value.Title));
    }
}