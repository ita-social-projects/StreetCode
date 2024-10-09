namespace Streetcode.BLL.DTO.Media.Images;

public class ImageDTO
{
    public int Id { get; set; }

    public string BlobName { get; set; } = null!;
    public string Base64 { get; set; } = null!;
    public string MimeType { get; set; } = null!;
    public ImageDetailsDto? ImageDetails { get; set; }
}
