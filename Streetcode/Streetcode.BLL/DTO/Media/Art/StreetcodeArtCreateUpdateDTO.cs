using Streetcode.BLL.DTO.Media.Create;
using Streetcode.BLL.DTO.Streetcode.Update.Interfaces;
using Streetcode.BLL.Enums;

namespace Streetcode.BLL.DTO.Media.Art
{
    public class StreetcodeArtCreateUpdateDTO : IModelState
    {
        public int Index { get; set; }
        public ArtCreateUpdateDTO Art { get; set; }
        public int StreetcodeId { get; set; }
        public ModelState ModelState { get; set; }
    }
}
