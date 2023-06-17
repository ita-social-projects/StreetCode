using Streetcode.BLL.DTO.Streetcode.Update.Interfaces;
using Streetcode.BLL.Enums;

namespace Streetcode.BLL.DTO.Timeline.Update
{
    public class TimelineItemCreateUpdateDTO : TimelineItemDTO, IModelState
    {
        public ModelState ModelState { get; set; } = ModelState.Updated;
        public IEnumerable<HistoricalContextCreateUpdateDTO> HistoricalContexts { get; set; }
    }
}
