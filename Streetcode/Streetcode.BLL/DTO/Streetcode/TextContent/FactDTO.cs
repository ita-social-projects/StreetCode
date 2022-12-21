using Streetcode.BLL.DTO.Media.Images;

namespace Streetcode.BLL.DTO.Streetcode.TextContent;

public class FactDTO
{
    public int Id;
    public string Title;
    public int ImageId;
    public ImageDTO Image;
    public string FactContent;
    public string Text;
    public IEnumerable<StreetcodeDTO> Streetcodes;
}