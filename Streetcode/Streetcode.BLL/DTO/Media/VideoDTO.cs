using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.DTO.Streetcode;

namespace Streetcode.BLL.DTO.Media;

public class VideoDTO
{
    public int Id { get; set; }
    public string? Description { get; set; }
    public UrlDTO Url { get; set; }
    public int StreetcodeId { get; set; }
}