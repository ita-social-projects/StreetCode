using Streetcode.BLL.DTO.AdditionalContent.Coordinates;
using Streetcode.BLL.DTO.Identifier;
using Streetcode.BLL.DTO.Streetcode;

namespace Streetcode.BLL.DTO.Toponyms;

public class ToponymDTO
{
    public IdentifierDTO Identifier;
    public string Description;
    public List<CoordinatesDTO> Coordinates;
    public List<StreetcodeDTO> Streetcodes;
}