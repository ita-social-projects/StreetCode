using Streetcode.BLL.DTO.Streetcode.Update.Interfaces;
using Streetcode.BLL.Enums;

namespace Streetcode.BLL.DTO.Toponyms
{
    public class StreetcodeToponymUpdateDTO : IModelState
    {
        public ModelState ModelState { get; set; } = ModelState.Updated;
        public int StreetcodeId { get; set; }
        public int ToponymId { get; set; }
    }
}
