using Streetcode.BLL.DTO.Streetcode.Update.Interfaces;
using Streetcode.BLL.Enums;

namespace Streetcode.BLL.DTO.Media.Video
{
  public class VideoUpdateDTO : VideoDTO, IModelState
  {
    public ModelState ModelState { get; set; }
  }
}
