using Streetcode.BLL.Factories.Event;
using Streetcode.DAL.Entities.Event;
using Streetcode.DAL.Enums;
using Xunit;

namespace Streetcode.XUnitTest.Factories.Event;
public class EventFactoryTests
{
    [Fact]
    public void CreateEvent_HistoricalEventType_ReturnHistoricalEvent()
    {
        // Arrange
        var eventTypeHistorical = EventTypeDiscriminators.HistoricalEventType;

        // Act
        var eventEntity = EventFactory.CreateEvent(eventTypeHistorical);

        // Assert
        Assert.IsType<HistoricalEvent>(eventEntity);
    }

    [Fact]
    public void CreateEvent_CustomlEventType_ReturnCustomEvent()
    {
        // Arrange
        var eventTypeCustom = EventTypeDiscriminators.CustomEventType;

        // Act
        var eventEntity = EventFactory.CreateEvent(eventTypeCustom);

        // Assert
        Assert.IsType<CustomEvent>(eventEntity);
    }
}