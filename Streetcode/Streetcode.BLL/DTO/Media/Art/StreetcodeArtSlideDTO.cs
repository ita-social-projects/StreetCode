using Streetcode.DAL.Enums;

namespace Streetcode.BLL.DTO.Media.Art;

public class StreetcodeArtSlideDTO
{
    public int Index { get; set; }
    public int StreetcodeId { get; set; }
    public StreetcodeArtSlide Template { get; set; }
    public IEnumerable<StreetcodeArtDTO>? StreetcodeArts { get; set; }
}