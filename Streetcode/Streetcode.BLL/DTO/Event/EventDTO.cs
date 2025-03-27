using Streetcode.DAL.Enums;
using Streetcode.BLL.DTO.Streetcode;

namespace Streetcode.BLL.DTO.Event
{
    public class EventDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string EventType { get; set; }
        public List<StreetcodeShortDTO>? Streetcodes { get; set; }
    }
}
