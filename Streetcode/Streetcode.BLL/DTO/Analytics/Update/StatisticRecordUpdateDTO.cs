using Streetcode.BLL.DTO.Streetcode.Update.Interfaces;
using Streetcode.BLL.Enums;

namespace Streetcode.BLL.DTO.Analytics.Update
{
    public class StatisticRecordUpdateDTO : StatisticRecordDTO, IModelState
    {
        public ModelState ModelState { get; set; } = ModelState.Updated;

    }
}
