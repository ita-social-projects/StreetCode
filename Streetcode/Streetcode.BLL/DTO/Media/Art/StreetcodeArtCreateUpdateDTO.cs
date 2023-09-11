using Streetcode.BLL.DTO.Media.Create;
using Streetcode.BLL.DTO.Streetcode.Update.Interfaces;
using Streetcode.BLL.Enums;

namespace Streetcode.BLL.DTO.Media.Art
{
    public class StreetcodeArtCreateUpdateDTO
    {
        public int Index { get; set; }
        public ArtCreateUpdateDTO Art { get; set; }
    }
}
