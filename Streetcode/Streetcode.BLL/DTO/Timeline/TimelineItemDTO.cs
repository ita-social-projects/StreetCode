namespace Streetcode.BLL.DTO.Timeline;

public class TimelineItemDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public DateTime Date { get; set; }
    public List<HistoricalContextDTO> HistoricalContexts { get; set; }
}
