using Streetcode.DAL.Enums;

namespace Streetcode.BLL.DTO.Media.Art;

public class StreetcodeArtSlideDto
{
    public int SlideId { get; set; }
    public int Index { get; set; }
    public int StreetcodeId { get; set; }
    public StreetcodeArtSlideTemplate Template { get; set; }
    public IEnumerable<StreetcodeArtDto> StreetcodeArts { get; set; } = new List<StreetcodeArtDto>();
}