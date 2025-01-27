using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streetcode.DAL.Enums
{
    public static class EventTypeDiscriminators
    {
        public static string EventBaseType { get => "Base"; }
        public static string HistoricalType { get => "Historical"; }
        public static string CustomEventType { get => "Custom"; }
        public static string DiscriminatorName { get => "EventType"; }

        public static string GetEventType(EventType eventType)
        {
            switch (eventType)
            {
                case EventType.Custom: return HistoricalType;
                case EventType.Historical: return CustomEventType;
                default: return EventBaseType;
            }
        }
    }
}
