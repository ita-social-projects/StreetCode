using System.ComponentModel.DataAnnotations;
using Streetcode.DAL.Entities.Partners;
using Streetcode.DAL.Entities.Streetcode;

namespace Streetcode.DAL.Entities.Timeline
{
    public class HistoricalContextTimeline
    {
        [Required]
        public int HistoricalContextId { get; set; }

        [Required]
        public int TimelineId { get; set; }

        public HistoricalContext? HistoricalContext { get; set; }

        public TimelineItem? TimelineItem { get; set; }
    }
}
