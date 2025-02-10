using Streetcode.DAL.Enums;
using Streetcode.BLL.DTO.Streetcode;

namespace Streetcode.BLL.DTO.Event
{
    public class EventDTO
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public EventType EventType { get; set; }
        public List<StreetcodeShortDTO>? Streetcodes { get; set; }
    }
}
