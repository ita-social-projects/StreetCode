using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.DTO.Sources;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.DTO.Streetcode.TextContent;

namespace Streetcode.BLL.DTO.Media.Images;

public class ImageDTO
{
    public int Id { get; set; }
    public string Alt { get; set; }
    public UrlDTO Url { get; set; }
    public IEnumerable<StreetcodeDTO> Streetcodes { get; set; }
    public IEnumerable<FactDTO> Facts { get; set; }
    public IEnumerable<SourceLinkCategoryDTO> Categories { get; set; }
    public ArtDTO Art { get; set; }
}