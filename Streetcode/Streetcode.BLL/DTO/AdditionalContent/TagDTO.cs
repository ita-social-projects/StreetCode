using Streetcode.BLL.DTO.Identifier;
using Streetcode.BLL.DTO.Streetcode;

namespace Streetcode.BLL.DTO.AdditionalContent;

public class TagDTO
{
    public IdentifierDTO Identifier;
    public IEnumerable<StreetcodeDTO> Streetcodes;
}