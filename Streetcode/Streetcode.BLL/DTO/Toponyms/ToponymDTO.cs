using Streetcode.BLL.DTO.AdditionalContent.Coordinates;
using Streetcode.BLL.DTO.Identifier;
using Streetcode.BLL.DTO.Streetcode;

namespace Streetcode.BLL.DTO.Toponyms;

public class ToponymDTO
{
    public IdentifierDTO Identifier;
    public string Description;
    public IEnumerable<CoordinateDTO> Coordinates;
    public IEnumerable<StreetcodeDTO> Streetcodes;
}