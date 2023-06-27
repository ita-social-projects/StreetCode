using Streetcode.BLL.DTO.AdditionalContent;

namespace Streetcode.BLL.DTO.Media.Audio;

public class AudioDTO
{
  public int Id { get; set; }
  public string? Description { get; set; }
  public string BlobName { get; set; }
  public string Base64 { get; set; }
  public string MimeType { get; set; }
}