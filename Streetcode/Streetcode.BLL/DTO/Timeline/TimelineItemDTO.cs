using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.DTO.Streetcode;

namespace Streetcode.BLL.DTO.Timeline;

public class TimelineItemDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime Date { get; set; }
    public ImageDTO Image { get; set; }
    public IEnumerable<HistoricalContextDTO> HistoricalContexts { get; set; }
    public IEnumerable<StreetcodeDTO> Streetcodes { get; set; }
}