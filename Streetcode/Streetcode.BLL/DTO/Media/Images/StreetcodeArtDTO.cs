using Streetcode.BLL.DTO.Streetcode;

namespace Streetcode.BLL.DTO.Media.Images
{
    public class StreetcodeArtDTO
    {
        public int Index { get; set; }

        public int StreetcodeId { get; set; }

        public StreetcodeDTO? Streetcode { get; set; }

        public int ArtId { get; set; }

        public ArtDTO? Art { get; set; }
    }
}
