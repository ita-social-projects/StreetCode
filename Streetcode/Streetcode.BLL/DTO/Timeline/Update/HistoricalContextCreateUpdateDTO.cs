using Streetcode.BLL.DTO.Streetcode.Update.Interfaces;
using Streetcode.BLL.Enums;

namespace Streetcode.BLL.DTO.Timeline.Update
{
    public class HistoricalContextCreateUpdateDto : HistoricalContextDto, IModelState
    {
        public int TimelineId { get; set; }
        public ModelState ModelState { get; set; } = ModelState.Updated;
    }
}
