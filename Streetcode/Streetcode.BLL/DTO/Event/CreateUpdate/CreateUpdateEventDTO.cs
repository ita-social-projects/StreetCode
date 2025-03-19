using Streetcode.DAL.Enums;

namespace Streetcode.BLL.DTO.Event.CreateUpdate
{
    public class CreateUpdateEventDTO
    {
        public string Title { get; set; } = null!;
        public DateTime Date { get; set; }
        public string? Description { get; set; }
        public List<int>? StreetcodeIds { get; set; }
        public string EventType { get; set; }
        public string? Location { get; set; }
        public string? Organizer { get; set; }
        public int? TimelineItemId { get; set; }
        public string? DateString { get; set; }
    }
}
