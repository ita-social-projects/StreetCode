using System.Linq.Expressions;
using System.Transactions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using Moq;
using Streetcode.BLL.DTO.Event;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Event.Update;
using Streetcode.DAL.Entities.Event;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.XUnitTest.Mocks;
using Xunit;
using EventEntity = Streetcode.DAL.Entities.Event.Event;

namespace Streetcode.XUnitTest.MediatRTests.Event;

public class UpdateEventTests
{
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IRepositoryWrapper> _mockRepository;
    private readonly Mock<ILoggerService> _mockLogger;
    private readonly MockCannotFindLocalizer _mockCanFindLocalizer;
    private readonly MockFailedToUpdateLocalizer _mockFailedToUpdateLocalizer;
    private readonly MockAnErrorOccurredLocalizer _mockAnErrorOccurredLocalizer;
    private readonly UpdateEventHandler _handler;

    public UpdateEventTests()
    {
        _mockMapper = new Mock<IMapper>();
        _mockRepository = new Mock<IRepositoryWrapper>();
        _mockLogger = new Mock<ILoggerService>();

        _mockCanFindLocalizer = new MockCannotFindLocalizer();
        _mockFailedToUpdateLocalizer = new MockFailedToUpdateLocalizer();
        _mockAnErrorOccurredLocalizer = new MockAnErrorOccurredLocalizer();

        _handler = new UpdateEventHandler(
            _mockMapper.Object,
            _mockRepository.Object,
            _mockLogger.Object,
            _mockFailedToUpdateLocalizer,
            _mockAnErrorOccurredLocalizer,
            _mockCanFindLocalizer);
    }

    [Fact]
    public async Task Handle_ShouldReturnEventId_WhenEventUpdatedSuccessfully()
    {
        // Arrange
        var request = new UpdateEventCommand(new UpdateEventDto { Id = 1, EventType = "Historical", TimelineItemId = 10 });
        var eventEntity = new HistoricalEvent { Id = 1, EventType = "Historical", TimelineItemId = 5 };
        SetupRepositoryMock(eventEntity, saveSuccess: true);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.True(result.IsSuccess);
            Assert.Equal(1, result.Value);
        });
    }

    [Fact]
    public async Task Handle_ShouldReturnFail_WhenEventNotFound()
    {
        // Arrange
        var eventId = 99;
        var request = new UpdateEventCommand(new UpdateEventDto { Id = eventId });
        string expectedErrorKey = "CannotFindEventWithCorrespondingId";
        string expectedErrorValue = _mockCanFindLocalizer[expectedErrorKey, eventId];

        SetupRepositoryMock(null, saveSuccess: false);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.False(result.IsSuccess);
            Assert.Contains(expectedErrorValue, result.Errors.Single().Message);
            _mockRepository.Verify(repo => repo.SaveChangesAsync(), Times.Never);
        });
    }

    [Fact]
    public async Task Handle_ShouldReturnFail_WhenEventTypeChangeFails()
    {
        // Arrange
        var request = new UpdateEventCommand(new UpdateEventDto { Id = 1, EventType = "Custom" });
        var eventEntity = new HistoricalEvent { Id = 1, EventType = "Historical" };
        string expectedErrorKey = "AnErrorOccurredWhileUpdatingEvent";
        string expectedException = "Failed to map event";
        string expectedErrorValue = _mockAnErrorOccurredLocalizer[expectedErrorKey, expectedException];

        SetupRepositoryMock(eventEntity, saveSuccess: true);

        _mockMapper.Setup(m => m.Map(It.IsAny<UpdateEventDto>(), It.IsAny<EventEntity>()))
                   .Throws(new Exception(expectedException));

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.False(result.IsSuccess);
            Assert.Contains(expectedErrorValue, result.Errors.Single().Message);
        });
    }

    [Fact]
    public async Task Handle_ShouldReturnFail_WhenStreetcodeUpdateFails()
    {
        // Arrange
        var request = new UpdateEventCommand(new UpdateEventDto { Id = 1, EventType = "Historical", StreetcodeIds = new List<int> { 1, 2 } });
        var eventEntity = new HistoricalEvent { Id = 1, EventType = "Historical", EventStreetcodes = new List<EventStreetcodes>() };
        string expectedErrorKey = "AnErrorOccurredWhileUpdatingEvent";
        string expectedException = "Database error";
        string expectedErrorValue = _mockAnErrorOccurredLocalizer[expectedErrorKey, expectedException];

        SetupRepositoryMock(eventEntity, saveSuccess: true);

        _mockRepository.Setup(repo => repo.EventStreetcodesRepository.CreateRangeAsync(It.IsAny<IEnumerable<EventStreetcodes>>()))
                       .ThrowsAsync(new Exception(expectedException));

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.False(result.IsSuccess);
            Assert.Contains(expectedErrorValue, result.Errors.Single().Message);
        });
    }

    private void SetupRepositoryMock(EventEntity? eventEntity, bool saveSuccess)
    {
        _mockRepository.Setup(r => r.BeginTransaction()).Returns(new TransactionScope(TransactionScopeAsyncFlowOption.Enabled));

        _mockRepository.Setup(repo => repo.EventRepository.Update(It.IsAny<EventEntity>()));
        _mockRepository.Setup(repo => repo.EventRepository.GetFirstOrDefaultAsync(
            It.IsAny<Expression<Func<EventEntity, bool>>>(),
            It.IsAny<Func<IQueryable<EventEntity>, IIncludableQueryable<EventEntity, object>>>()))
            .ReturnsAsync(eventEntity);

        _mockRepository.Setup(repo => repo.SaveChangesAsync()).ReturnsAsync(saveSuccess ? 1 : 0);
    }
}
