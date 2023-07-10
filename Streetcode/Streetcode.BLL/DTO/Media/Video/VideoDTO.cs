using Streetcode.BLL.DTO.AdditionalContent;

namespace Streetcode.BLL.DTO.Media.Video;

public class VideoDTO
{
  public int Id { get; set; }
  public string? Description { get; set; }
  public string? Url { get; set; }
  public int StreetcodeId { get; set; }
}
