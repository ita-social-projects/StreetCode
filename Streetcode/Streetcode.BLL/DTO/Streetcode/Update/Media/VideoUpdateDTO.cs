using Streetcode.BLL.DTO.Media;
using Streetcode.BLL.DTO.Streetcode.Update.Interfaces;
using Streetcode.BLL.Enums;

namespace Streetcode.BLL.DTO.Streetcode.Update.Media
{
    public class VideoUpdateDTO : VideoDTO, IModelState
  {
        public ModelState ModelState { get; set; }
    }
}
