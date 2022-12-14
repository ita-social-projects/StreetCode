using Streetcode.BLL.DTO.Streetcode;

namespace Streetcode.BLL.DTO.Media.Images;

public class ArtDTO
{
    public int Id;
    public string Description;
    public IEnumerable<StreetcodeDTO> Streetcodes;
    public int ImageId;
    public ImageDTO Image;
}