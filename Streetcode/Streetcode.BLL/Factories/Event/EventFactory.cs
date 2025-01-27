using Streetcode.DAL.Entities.Event;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.Factories.Event
{
    public static class EventFactory
    {
        public static DAL.Entities.Event.Event CreateEvent(EventType eventType)
        {
            switch(eventType)
            {
                case EventType.Custom:
                    return new CustomEvent();
                case EventType.Historical:
                    return new HistoricalEvent();
                default:
                    throw new ArgumentException($"Unsupported streetcode type: {eventType}", nameof(eventType));
            }
        }
    }
}
