using Streetcode.BLL.DTO.Streetcode.Update.Interfaces;
using Streetcode.BLL.Enums;

namespace Streetcode.BLL.DTO.Partners.Update
{
    public class PartnersUpdateDTO : IModelState
    {
        public int StreetcodeId { get; set; }
        public int PartnerId { get; set; }
        public ModelState ModelState { get; set; } = ModelState.Updated;
    }
}
