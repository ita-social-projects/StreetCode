using Streetcode.DAL.Entities.Event;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.Factories.Event
{
    public static class EventFactory
    {
        public static DAL.Entities.Event.Event CreateEvent(string eventType)
        {
            switch(eventType)
            {
                case "Custom":
                    return new CustomEvent();
                case "Historical":
                    return new HistoricalEvent();
                default:
                    throw new ArgumentException($"Unsupported streetcode type: {eventType}", nameof(eventType));
            }
        }
    }
}
