using Streetcode.DAL.Enums;

namespace Streetcode.BLL.DTO.Timeline;

public class TimelineItemDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime Date { get; set; }
    public DateViewPattern DateViewPattern { get; set; }
    public IEnumerable<HistoricalContextDTO> HistoricalContexts { get; set; } = new List<HistoricalContextDTO>();
}
