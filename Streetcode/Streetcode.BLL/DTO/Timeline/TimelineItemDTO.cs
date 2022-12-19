using Streetcode.BLL.DTO.Identifier;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.DTO.Streetcode;

namespace Streetcode.BLL.DTO.Timeline;

public class TimelineItemDTO
{
    public IdentifierDTO Identifier;
    public string Description;
    public DateTime Date;
    public ImageDTO Image;
    public IEnumerable<HistoricalContextDTO> HistoricalContext;
    public IEnumerable<StreetcodeDTO> Streetcodes;
}