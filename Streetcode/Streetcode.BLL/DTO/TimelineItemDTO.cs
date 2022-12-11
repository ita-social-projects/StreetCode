
namespace DTO;

public class TimelineItemDTO 
{

    public int Id;

    public string Title;

    public string Description;

    public int Date;

    public ImageDTO Image;

    public HashSet<HistoricalContextDTO> HistoricalContext;

}