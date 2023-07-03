using Streetcode.BLL.DTO.Streetcode.Update.Interfaces;
using Streetcode.BLL.Enums;

namespace Streetcode.BLL.DTO.Toponyms
{
    public class StreetcodeToponymUpdateDTO : IModelState
    {
        public int StreetcodeId { get; set; }
        public int ToponymId { get; set; }
        public string StreetName { get; set; }
        public ModelState ModelState { get; set; } = ModelState.Updated;
    }
}
