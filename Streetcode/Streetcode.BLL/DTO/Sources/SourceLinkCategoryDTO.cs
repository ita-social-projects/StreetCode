using Streetcode.BLL.DTO.Media.Images;

namespace Streetcode.BLL.DTO.Sources;

public class SourceLinkCategoryDTO
{
    public int Id;
    public string Title;
    public int ImageId;
    public ImageDTO Image;
    public IEnumerable<SourceLinkDTO> SourceLinks;
}