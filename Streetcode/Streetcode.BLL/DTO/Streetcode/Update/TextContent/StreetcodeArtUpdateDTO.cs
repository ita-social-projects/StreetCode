using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.DTO.Streetcode.Update.Interface;

namespace Streetcode.BLL.DTO.Streetcode.Update.TextContent
{
    public class StreetcodeArtUpdateDTO : StreetcodeArtDTO, IChanged
    {
        public bool? Changed { get; set; }
    }
}
