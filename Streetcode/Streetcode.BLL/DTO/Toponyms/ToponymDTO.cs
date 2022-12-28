using Streetcode.BLL.DTO.AdditionalContent.Coordinates.Types;
using Streetcode.BLL.DTO.Streetcode;

namespace Streetcode.BLL.DTO.Toponyms;

public class ToponymDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public IEnumerable<ToponymCoordinateDTO> Coordinates { get; set; }
    public IEnumerable<StreetcodeDTO> Streetcodes { get; set; }
}