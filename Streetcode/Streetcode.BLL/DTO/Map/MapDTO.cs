using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.DTO.Toponyms;

namespace Streetcode.BLL.DTO.Map;

public class MapDTO
{
    public ImageDTO Background { get; set; }
    public IEnumerable<ToponymDTO> Toponyms { get; set; }
    public IEnumerable<StreetcodeDTO> Streetcodes { get; set; }
}