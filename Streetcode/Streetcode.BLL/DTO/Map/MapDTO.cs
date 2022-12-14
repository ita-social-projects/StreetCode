using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.DTO.Toponyms;

namespace Streetcode.BLL.DTO.Map;

public class MapDTO
{
    public ImageDTO Background;
    public IEnumerable<ToponymDTO> Toponyms;
    public IEnumerable<StreetcodeDTO> Streetcodes;
}