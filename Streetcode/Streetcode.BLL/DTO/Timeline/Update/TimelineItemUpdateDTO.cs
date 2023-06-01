using Streetcode.BLL.DTO.Streetcode.Update.Interfaces;
using Streetcode.BLL.Enums;

namespace Streetcode.BLL.DTO.Timeline.Update
{
    public class TimelineItemUpdateDTO : TimelineItemDTO, IModelState
    {
        public ModelState ModelState { get; set; } = ModelState.Updated;
        public IEnumerable<HistoricalContextUpdateDTO> HistoricalContexts { get; set; }
    }
}
