using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.DTO.Streetcode.TextContent;

namespace Streetcode.BLL.DTO.Media.Images;

public class ImageDTO
{
    public int Id;
    public string Alt;
    public UrlDTO Url;
    public IEnumerable<StreetcodeDTO> Streetcodes;
    public IEnumerable<FactDTO> Facts;
    public ArtDTO Art;
}