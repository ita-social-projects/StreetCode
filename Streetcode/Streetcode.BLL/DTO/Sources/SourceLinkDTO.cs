using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.DTO.Identifier;
using Streetcode.BLL.DTO.Streetcode;

namespace Streetcode.BLL.DTO.Sources;

public class SourceLinkDTO
{
    public IdentifierDTO Identifier;
    public UrlDTO Url;
    public int StreetcodeId;
    public StreetcodeDTO Streetcode;
}