using Streetcode.BLL.DTO.Media.Images;
namespace Streetcode.BLL.DTO.Sources;

public class SourceLinkCategoryDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public int ImageId { get; set; }
    public ImageDTO Image { get; set; } = null!;
}