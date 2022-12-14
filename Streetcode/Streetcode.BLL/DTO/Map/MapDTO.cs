using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.DTO.Toponyms;

namespace Streetcode.BLL.DTO.Map;

public class MapDTO
{
    public ImageDTO Background;
    public List<ToponymDTO> Toponyms;
    public List<StreetcodeDTO> Streetcodes;
}