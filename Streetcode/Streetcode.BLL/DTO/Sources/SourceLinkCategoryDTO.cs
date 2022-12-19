using Streetcode.BLL.DTO.Identifier;
using Streetcode.BLL.DTO.Media.Images;

namespace Streetcode.BLL.DTO.Sources;

public class SourceLinkCategoryDTO
{
    public IdentifierDTO Identifier;

    public int ImageId;

    public ImageDTO Image;

    public IEnumerable<SourceLinkDTO> SourceLink;
}