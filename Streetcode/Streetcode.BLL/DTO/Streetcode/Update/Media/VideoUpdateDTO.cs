using Streetcode.BLL.DTO.Media;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.Update.Interfaces;

namespace Streetcode.BLL.DTO.Streetcode.Update.Media
{
    public class VideoUpdateDTO : VideoDTO, IDeleted
    {
        public bool IsDeleted { get; set; } = false;
    }
}
