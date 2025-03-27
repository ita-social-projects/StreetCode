using System.Transactions;
using AutoMapper;
using Moq;
using Streetcode.BLL.DTO.Event.CreateUpdate;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Event.Create;
using Streetcode.DAL.Entities.Event;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.XUnitTest.Mocks;
using Xunit;
using EventEntity = Streetcode.DAL.Entities.Event.Event;

namespace Streetcode.XUnitTest.MediatRTests.Event;

public class CreateEventTests
{
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IRepositoryWrapper> _mockRepository;
    private readonly Mock<ILoggerService> _mockLogger;
    private readonly MockFailedToCreateLocalizer _mockFailedToCreateLocalize;
    private readonly MockAnErrorOccurredLocalizer _mockAnErrorOccurredLocalizer;
    private readonly CreateEventHandler _handler;

    public CreateEventTests()
    {
        _mockMapper = new Mock<IMapper>();
        _mockRepository = new Mock<IRepositoryWrapper>();
        _mockLogger = new Mock<ILoggerService>();

        _mockFailedToCreateLocalize = new MockFailedToCreateLocalizer();
        _mockAnErrorOccurredLocalizer = new MockAnErrorOccurredLocalizer();

        _handler = new CreateEventHandler(
            _mockMapper.Object,
            _mockRepository.Object,
            _mockLogger.Object,
            _mockFailedToCreateLocalize,
            _mockAnErrorOccurredLocalizer);
    }

    [Fact]
    public async Task Handle_ShouldCreateHistoricalEventSuccessfully()
    {
        // Arrange
        var request = new CreateEventCommand(new CreateUpdateEventDto { EventType = "Historical", TimelineItemId = 1 });
        var historicalEvent = new HistoricalEvent();

        SetupRepositoryMock(saveSuccess: true);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.True(result.IsSuccess);
            Assert.Equal(1, result.Value);
            _mockRepository.Verify(r => r.EventRepository.Create(It.IsAny<HistoricalEvent>()), Times.Once);
            _mockRepository.Verify(r => r.SaveChangesAsync(), Times.AtLeastOnce);
        });
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenEventCreationFails()
    {
        // Arrange
        var request = new CreateEventCommand(new CreateUpdateEventDto { EventType = "Custom" });
        string expectedErrorKey = "FailedToCreateEvent";
        string expectedErrorValue = _mockFailedToCreateLocalize[expectedErrorKey];

        SetupRepositoryMock(false);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Contains(result.Errors, e => e.Message.Contains(expectedErrorValue));
        });
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenExceptionOccurs()
    {
        // Arrange
        var request = new CreateEventCommand(new CreateUpdateEventDto { EventType = "Historical" });
        string expectedErrorKey = "AnErrorOccurredWhileCreatingEvent";
        string expectedException = "Database error";
        string expectedErrorValue = _mockAnErrorOccurredLocalizer[expectedErrorKey, expectedException];

        SetupRepositoryMock(false, new Exception(expectedException));

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.NotNull(result);
            Console.WriteLine(result);
            Assert.False(result.IsSuccess);
            Assert.Contains(expectedErrorValue, result.Errors.First().Message);
        });
    }

    private void SetupRepositoryMock(bool saveSuccess, Exception? exception = null)
    {
        _mockRepository.Setup(r => r.BeginTransaction()).Returns(new TransactionScope(TransactionScopeAsyncFlowOption.Enabled));

        if (exception != null)
        {
            _mockRepository.Setup(repo => repo.EventRepository.Create(It.IsAny<EventEntity>()))
                .Throws(exception);
        }
        else
        {
            _mockRepository.Setup(repo => repo.EventRepository.Create(It.IsAny<EventEntity>()))
                .Callback<EventEntity>(e => e.Id = 1);

            _mockRepository.Setup(repo => repo.SaveChangesAsync()).ReturnsAsync(saveSuccess ? 1 : 0);
        }
    }
}