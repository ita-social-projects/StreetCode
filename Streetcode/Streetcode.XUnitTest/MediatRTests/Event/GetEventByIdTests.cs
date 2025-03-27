using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Event;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Event.GetById;
using Streetcode.DAL.Entities.Event;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.XUnitTest.Mocks;
using Xunit;
using EventEntity = Streetcode.DAL.Entities.Event.Event;

namespace Streetcode.XUnitTest.MediatRTests.Event;

public class GetEventByIdTests
{
    private readonly Mock<IRepositoryWrapper> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILoggerService> _mockLogger;
    private readonly MockCannotFindLocalizer _mockCannotFindLocalizer;
    private readonly GetEventByIdHandler _handler;

    public GetEventByIdTests()
    {
        _mockRepository = new Mock<IRepositoryWrapper>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILoggerService>();

        _mockCannotFindLocalizer = new MockCannotFindLocalizer();

        _handler = new GetEventByIdHandler(
            _mockMapper.Object,
            _mockRepository.Object,
            _mockLogger.Object,
            _mockCannotFindLocalizer);
    }

    [Theory]
    [InlineData(1)]
    public async Task Handle_ShouldReturnMappedEvent_WhenEventExist(int id)
    {
        // Arrange
        var testEvent = new HistoricalEvent { Id = 1, EventType = "Historical" };
        var testEventDto = new HistoricalEventDto { Id = 1, EventType = "Historical" };
        var request = new GetEventByIdQuery(id);

        SetupRepositoryMock(testEvent);

        _mockMapper.Setup(m => m.Map<HistoricalEventDto>(It.IsAny<EventEntity>()))
            .Returns(testEventDto);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal(testEventDto.Id, result.Value.Id);
            Assert.Equal(testEventDto.EventType, result.Value.EventType);
        });
    }

    [Theory]
    [InlineData(99)]
    public async Task Handle_ShouldReturnError_WhenEventNotFound(int id)
    {
        // Arrange
        var request = new GetEventByIdQuery(id);
        string expectedErrorKey = "CannotFindEventWithCorrespondingId";
        string expectedErrorValue = _mockCannotFindLocalizer[expectedErrorKey, request.id];

        SetupRepositoryMock(null);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Contains(expectedErrorValue, result.Errors.Single().Message);
        });
    }

    private void SetupRepositoryMock(EventEntity? eventEntity)
    {
        _mockRepository.Setup(repo => repo.EventRepository.GetFirstOrDefaultAsync(
            It.IsAny<Expression<Func<EventEntity, bool>>>(),
            It.IsAny<Func<IQueryable<EventEntity>, IIncludableQueryable<EventEntity, object>>>()))
            .ReturnsAsync(eventEntity);
    }
}