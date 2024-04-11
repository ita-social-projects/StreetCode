namespace Streetcode.BLL.DTO.Media.Images
{
    public class ImageDTOCreateEntities
    {
        public string? BlobName { get; set; }
        public string? Base64 { get; set; }
        public string? MimeType { get; set; }
        public ImageDetailsDto? ImageDetails { get; set; }
    }
}
