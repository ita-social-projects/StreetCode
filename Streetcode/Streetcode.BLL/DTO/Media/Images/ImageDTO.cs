using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.DTO.Identifier;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.DTO.Streetcode.TextContent;

namespace Streetcode.BLL.DTO.Media.Images;

public class ImageDTO
{
    public IdentifierDTO Identifier;
    public string Alt;
    public UrlDTO Url;
    public List<StreetcodeDTO> Streetcodes;
    public List<FactDTO> Facts;
    public ArtDTO Art;
}