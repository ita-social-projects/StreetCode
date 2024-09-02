using Streetcode.BLL.DTO.AdditionalContent.Coordinates.Types;
using Streetcode.BLL.DTO.Streetcode;

namespace Streetcode.BLL.DTO.Toponyms;

public class ToponymDTO
{
    public int Id { get; set; }
    public string Oblast { get; set; } = null!;
    public string AdminRegionOld { get; set; } = null!;
    public string AdminRegionNew { get; set; } = null!;
    public string Gromada { get; set; } = null!;
    public string Community { get; set; } = null!;
    public string StreetName { get; set; } = null!;
    public string StreetType { get; set; } = null!;

    public ToponymCoordinateDTO Coordinate { get; set; } = null!;
    public IEnumerable<StreetcodeDTO> Streetcodes { get; set; } = new List<StreetcodeDTO>();
}