using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.DTO.Identifier;
using Streetcode.BLL.DTO.Streetcode;

namespace Streetcode.BLL.DTO.Media;

public class AudioDTO
{
    public IdentifierDTO Identifier;
    public string Description;
    public UrlDTO Url;
    public int StreetcodeId;
    public IEnumerable<StreetcodeDTO> Streetcodes;
}