using Streetcode.BLL.DTO.Media.Create;
using Streetcode.BLL.DTO.Streetcode.Update.Interfaces;
using Streetcode.BLL.Enums;

namespace Streetcode.BLL.DTO.Media.Art
{
    public class ArtUpdateDTO : ArtCreateDTO, IModelState
    {
        public ModelState ModelState { get; set; }
    }
}
