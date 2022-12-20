using Streetcode.BLL.DTO.Identifier;
using Streetcode.BLL.DTO.Media.Images;

namespace Streetcode.BLL.DTO.Streetcode.TextContent;

public class FactDTO
{
    public IdentifierDTO Identifier; // TODO reverse changes to "id-title" pattern
    public int ImageId;
    public ImageDTO Image;
    public string FactContent;
    public string Text;
    public IEnumerable<StreetcodeDTO> Streetcodes;
}