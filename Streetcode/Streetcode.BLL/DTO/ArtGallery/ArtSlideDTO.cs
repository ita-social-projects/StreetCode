using Streetcode.BLL.Enums;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.DTO.ArtGallery.ArtSlide;

public class ArtSlideDTO
{
    public int Index { get; set; }
    public int? StreetcodeId { get; set; }
    public StreetcodeArtSlideTemplate Template { get; set; }
    public ModelState ModelState { get; set; }
    public IEnumerable<ArtForArtSlideDTO> StreetcodeArts { get; set; } = new List<ArtForArtSlideDTO>();
}