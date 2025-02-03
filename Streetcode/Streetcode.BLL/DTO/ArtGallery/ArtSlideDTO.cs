using Streetcode.BLL.Enums;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.DTO.ArtGallery.ArtSlide;

public class ArtSlideDto
{
    public int Index { get; set; }
    public int? StreetcodeId { get; set; }
    public StreetcodeArtSlideTemplate Template { get; set; }
    public ModelState ModelState { get; set; }
    public IEnumerable<ArtForArtSlideDto> StreetcodeArts { get; set; } = new List<ArtForArtSlideDto>();
}