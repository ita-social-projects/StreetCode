using Streetcode.BLL.DTO.AdditionalContent;

namespace Streetcode.BLL.DTO.Media;

public class VideoDTO
{
    public int Id { get; set; }
    public string Description { get; set; } = null!;
    public string Url { get; set; } = null!;
    public int StreetcodeId { get; set; }
}