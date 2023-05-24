using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.Update.Interfaces;

namespace Streetcode.BLL.DTO.Streetcode.Update.TextContent
{
    public class StreetcodeArtUpdateDTO : StreetcodeArtDTO, IDeleted
    {
        public bool IsDeleted { get; set; }
    }
}
