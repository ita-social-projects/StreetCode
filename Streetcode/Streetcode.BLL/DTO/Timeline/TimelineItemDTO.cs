using Streetcode.BLL.DTO.Identifier;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.DTO.Streetcode;

namespace Streetcode.BLL.DTO.Timeline;

public class TimelineItemDTO
{
    public IdentifierDTO Identifier;
    public string Description;
    public int Date;
    public ImageDTO Image;
    public List<HistoricalContextDTO> HistoricalContext;
    public List<StreetcodeDTO> Streetcodes;
}