using System.Linq.Expressions;
using Moq;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Event.Delete;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.XUnitTest.Mocks;
using Xunit;
using EventEntity = Streetcode.DAL.Entities.Event.Event;

namespace Streetcode.XUnitTest.MediatRTests.Event;

public class DeleteEventTests
{
    private readonly Mock<IRepositoryWrapper> _mockRepository;
    private readonly Mock<ILoggerService> _mockLogger;
    private readonly MockCannotFindLocalizer _mockCannotFindLocalizer;
    private readonly MockFailedToDeleteLocalizer _mockFailedToDeleteLocalizer;
    private readonly DeleteEventHandler _handler;

    public DeleteEventTests()
    {
        _mockRepository = new Mock<IRepositoryWrapper>();
        _mockLogger = new Mock<ILoggerService>();

        _mockCannotFindLocalizer = new MockCannotFindLocalizer();
        _mockFailedToDeleteLocalizer = new MockFailedToDeleteLocalizer();

        _handler = new DeleteEventHandler(
            _mockRepository.Object,
            _mockLogger.Object,
            _mockCannotFindLocalizer,
            _mockFailedToDeleteLocalizer);
    }

    [Fact]
    public async Task Handle_ShouldDeleteEvent_WhenEventExist()
    {
        // Arrange
        var eventId = 1;
        var eventEntity = new EventEntity { Id = eventId };
        var request = new DeleteEventCommand(eventId);

        SetupRepositoryMock(eventEntity, 1);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.True(result.IsSuccess);
            Assert.Equal(eventId, result.Value);
            _mockRepository.Verify(repo => repo.EventRepository.Delete(eventEntity), Times.Once);
            _mockRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        });
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenEventDoesNotExist()
    {
        // Arrange
        var eventId = 99;
        var request = new DeleteEventCommand(eventId);
        string expectedErrorKey = "CannotFindEventWithCorrespondingId";
        string expectedErrorValue = _mockCannotFindLocalizer[expectedErrorKey, eventId];

        SetupRepositoryMock(null, 1);

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
    public async Task Handle_ShouldReturnError_WhenSaveChangesFails()
    {
        // Arrange
        var eventId = 1;
        var eventEntity = new EventEntity { Id = eventId };
        var request = new DeleteEventCommand(eventId);
        string expectedErrorKey = "FailedToDeleteEvent";
        string expectedErrorValue = _mockFailedToDeleteLocalizer[expectedErrorKey];

        SetupRepositoryMock(eventEntity, -1);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.False(result.IsSuccess);
            Assert.Contains(expectedErrorValue, result.Errors.Single().Message);
        });
    }

    private void SetupRepositoryMock(EventEntity? eventEntity, int saveChangesResult)
    {
        _mockRepository
            .Setup(repo => repo.EventRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<EventEntity, bool>>>(), null))
            .ReturnsAsync(eventEntity);

        if (eventEntity != null)
        {
            _mockRepository
                .Setup(repo => repo.EventRepository.Delete(eventEntity));
        }

        _mockRepository
            .Setup(repo => repo.SaveChangesAsync())
            .ReturnsAsync(saveChangesResult);
    }
}