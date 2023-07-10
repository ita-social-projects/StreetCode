using Streetcode.BLL.DTO.Media.Images;

namespace Streetcode.BLL.DTO.Media.Art;

public class ArtDTO
{
    public int Id { get; set; }
    public string? Description { get; set; }
    public string? Title { get; set; }
    public int ImageId { get; set; }
    public ImageDTO? Image { get; set; }
}
