using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Streetcode.DAL.Entities.Timeline;

namespace Streetcode.DAL.Entities.Event
{
    public class HistoricalEvent : Event
    {
        public int? TimelineItemId { get; set; }
        public TimelineItem? TimelineItem { get; set; }
    }
}
