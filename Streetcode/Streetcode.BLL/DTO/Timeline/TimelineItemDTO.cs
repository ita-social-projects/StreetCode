using Streetcode.DAL.Enums;

namespace Streetcode.BLL.DTO.Timeline;

public class TimelineItemDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime Date { get; set; }
    public DateViewPattern DateViewPattern { get; set; }
    public IEnumerable<HistoricalContextDto> HistoricalContexts { get; set; } = new List<HistoricalContextDto>();
}
