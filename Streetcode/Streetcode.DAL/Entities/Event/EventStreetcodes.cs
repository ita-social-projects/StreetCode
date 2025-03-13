using System.ComponentModel.DataAnnotations;
using Streetcode.DAL.Entities.Streetcode;

namespace Streetcode.DAL.Entities.Event
{
    public class EventStreetcodes
    {
        [Required]
        public int EventId { get; set; }
        [Required]
        public int StreetcodeId { get; set; }
        public Event? Event { get; set; }
        public StreetcodeContent? StreetcodeContent { get; set; }
    }
}
