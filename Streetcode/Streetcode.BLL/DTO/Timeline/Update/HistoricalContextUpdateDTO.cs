using Streetcode.BLL.DTO.Streetcode.Update.Interfaces;
using Streetcode.BLL.Enums;

namespace Streetcode.BLL.DTO.Timeline.Update
{
    public class HistoricalContextUpdateDTO : HistoricalContextDTO, IModelState
    {
        public ModelState ModelState { get; set; } = ModelState.Updated;
    }
}
