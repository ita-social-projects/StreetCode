namespace Streetcode.BLL.DTO.Timeline;

public class HistoricalContextDTO
{
    public int Id;
    public string Title;
    public IEnumerable<TimelineItemDTO> TimelineItems;
}