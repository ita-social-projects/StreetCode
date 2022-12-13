using Streetcode.BLL.DTO.Media.Images;

namespace Streetcode.BLL.DTO.Timeline;

public class TimelineItemDTO
{

    public int Id;

    public string Title;

    public string Description;

    public int Date;

    public ImageDTO Image;

    public List<HistoricalContextDTO> HistoricalContext;

}