using Streetcode.BLL.DTO.Identifier;

namespace Streetcode.BLL.DTO.Streetcode.TextContent;

public class TextDTO
{
    public IdentifierDTO Identifier;
    public string TextContent;
    public int StreetcodeId;
    public StreetcodeDTO Streetcode;
}