using Streetcode.BLL.DTO.Streetcode;

namespace Streetcode.BLL.DTO.Media.Images;

public class ArtDTO
{
    public int Id { get; set; }
    public string? Description { get; set; }
    public IEnumerable<StreetcodeDTO> Streetcodes { get; set; }
    public int ImageId { get; set; }
    public ImageDTO? Image { get; set; }
}