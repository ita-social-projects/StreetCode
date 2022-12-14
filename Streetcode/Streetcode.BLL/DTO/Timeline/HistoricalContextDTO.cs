using Streetcode.BLL.DTO.Identifier;

namespace Streetcode.BLL.DTO.Timeline;

public class HistoricalContextDTO
{
    public IdentifierDTO Identifier;
    public List<TimelineItemDTO> TimelineItems;
}