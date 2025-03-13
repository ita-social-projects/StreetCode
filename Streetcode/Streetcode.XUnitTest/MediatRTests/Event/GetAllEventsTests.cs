using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Event;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Event.GetAll;
using Streetcode.DAL.Entities.Event;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Helpers;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.XUnitTest.Mocks;
using Xunit;
using EventEntity = Streetcode.DAL.Entities.Event.Event;

namespace Streetcode.XUnitTest.MediatRTests.Event;
public class GetAllEventsTests
{
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IRepositoryWrapper> _mockRepository;
    private readonly Mock<ILoggerService> _mockLogger;
    private readonly MockCannotFindLocalizer _mockCannotFindLocalizer;
    private readonly GetAllEventsHandler _handler;

    public GetAllEventsTests()
    {
        _mockMapper = new Mock<IMapper>();
        _mockRepository = new Mock<IRepositoryWrapper>();
        _mockLogger = new Mock<ILoggerService>();

        _mockCannotFindLocalizer = new MockCannotFindLocalizer();

        _handler = new GetAllEventsHandler(
            _mockMapper.Object,
            _mockRepository.Object,
            _mockLogger.Object,
            _mockCannotFindLocalizer);
    }

    [Fact]
    public async Task Handle_ShouldReturnEvents_Successfully()
    {
        // Arrange
        var query = new GetAllEventsQuery(null, 1, 10);
        var events = GetTestEvents();
        var paginationResponse = PaginationResponse<EventEntity>.Create(events.AsQueryable(), query.page, query.pageSize);

        SetupRepositoryMock(paginationResponse);
        SetupMapperMock();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(events.Count, result.Value.Events.Count());
            Assert.Equal(paginationResponse.TotalItems, result.Value.TotalAmount);
        });
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenNoEventsFound()
    {
        // Arrange
        var query = new GetAllEventsQuery(null, 1, 10);
        PaginationResponse<EventEntity>? paginationResponse = null;

        string expectedErrorKey = "CannotFindAnyEvents";
        string expectedErrorValue = _mockCannotFindLocalizer[expectedErrorKey];

        SetupRepositoryMock(paginationResponse);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Contains(expectedErrorValue, result.Errors.Single().Message);
        });
    }

    private void SetupRepositoryMock(PaginationResponse<EventEntity>? paginationResponse)
    {
        _mockRepository.Setup(r => r.EventRepository.GetAllPaginated(
            It.IsAny<ushort?>(),
            It.IsAny<ushort?>(),
            null,
            null,
            It.IsAny<Func<IQueryable<EventEntity>, IIncludableQueryable<EventEntity, object>>?>(),
            null,
            It.IsAny<Expression<Func<EventEntity, object>>?>()))
        .Returns(paginationResponse);
    }

    private void SetupMapperMock()
    {
        _mockMapper.Setup(m => m.Map<It.IsAnyType>(It.IsAny<object>()))
            .Returns((object source) =>
            {
                if (source is HistoricalEvent historicalEvent)
                {
                    return new HistoricalEventDTO
                    {
                        Id = historicalEvent.Id,
                        EventType = historicalEvent.EventType,
                        Streetcodes = MapStreetcodes(historicalEvent.EventStreetcodes)
                    };
                }
                else if (source is CustomEvent customEvent)
                {
                    return new CustomEventDTO
                    {
                        Id = customEvent.Id,
                        EventType = customEvent.EventType,
                        Streetcodes = MapStreetcodes(customEvent.EventStreetcodes)
                    };
                }
                else if (source is EventEntity @event)
                {
                    return new EventDTO
                    {
                        Id = @event.Id,
                        EventType = @event.EventType,
                        Streetcodes = MapStreetcodes(@event.EventStreetcodes)
                    };
                }
                else
                {
                    return null!;
                }
            });
    }

    private List<StreetcodeShortDTO> MapStreetcodes(List<EventStreetcodes>? eventStreetcodes)
    {
        return eventStreetcodes?
            .Where(es => es.StreetcodeContent != null)
            .Select(es => new StreetcodeShortDTO
            {
                Id = es.StreetcodeContent!.Id,
                Title = es.StreetcodeContent.Title
            }).ToList() ?? new List<StreetcodeShortDTO>();
    }

    private List<EventEntity> GetTestEvents()
    {
        return new List<EventEntity>
        {
            new HistoricalEvent
            {
                Id = 1,
                EventType = "HistoricalEvent",
                EventStreetcodes = new List<EventStreetcodes>
                {
                    new EventStreetcodes
                    {
                        StreetcodeContent = new StreetcodeContent
                        {
                            Id = 1,
                            Title = "Історичний стріткод"
                        }
                    }
                }
            },
            new CustomEvent
            {
                Id = 2,
                EventType = "CustomEvent",
                EventStreetcodes = new List<EventStreetcodes>
                {
                    new EventStreetcodes
                    {
                        StreetcodeContent = new StreetcodeContent
                        {
                            Id = 2,
                            Title = "Користувацький стріткод"
                        }
                    }
                }
            }
        };
    }
}
