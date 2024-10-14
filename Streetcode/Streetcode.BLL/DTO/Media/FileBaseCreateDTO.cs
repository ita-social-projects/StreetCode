namespace Streetcode.BLL.DTO.Media;

public class FileBaseCreateDTO
{
    public string Title { get; set; } = null!;
    public string BaseFormat { get; set; } = null!;
    public string MimeType { get; set; } = null!;
    public string Extension { get; set; } = null!;
}
