using Streetcode.BLL.DTO.AdditionalContent.Coordinates;
using Streetcode.BLL.DTO.Streetcode;

namespace Streetcode.BLL.DTO.Toponyms;

public class ToponymDTO
{
    public int Id;
    public string Title;
    public string Description;
    public IEnumerable<ToponymCoordinateDTO> Coordinates;
    public IEnumerable<StreetcodeDTO> Streetcodes;
}