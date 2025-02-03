namespace Streetcode.BLL.DTO.Media.Audio;

public class AudioDto
{
  public int Id { get; set; }
  public string BlobName { get; set; } = null!;
  public string Base64 { get; set; } = null!;
  public string MimeType { get; set; } = null!;
}