using Streetcode.BLL.DTO.Streetcode.Update.Interfaces;
using Streetcode.BLL.Enums;

namespace Streetcode.BLL.DTO.Timeline.Update
{
    public class HistoricalContextCreateUpdateDTO : HistoricalContextDTO, IModelState
    {
        public int TimelineId { get; set; }
        public ModelState ModelState { get; set; } = ModelState.Updated;
    }
}
