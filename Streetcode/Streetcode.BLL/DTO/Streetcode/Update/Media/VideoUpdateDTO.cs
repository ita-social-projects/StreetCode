using Streetcode.BLL.DTO.Media;
using Streetcode.BLL.DTO.Streetcode.Update.Interface;

namespace Streetcode.BLL.DTO.Streetcode.Update.Media
{
    public class VideoUpdateDTO : VideoDTO, IChanged
    {
        public bool? Changed { get; set; }
    }
}
