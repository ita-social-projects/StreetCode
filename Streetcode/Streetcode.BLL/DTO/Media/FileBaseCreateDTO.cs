namespace Streetcode.BLL.DTO.Media;

public class FileBaseCreateDTO
{
    public int StreetcodeId { get; set; }
    public string BaseFormat { get; set; }
    public string MimeType { get; set; }
    public string Extension { get; set; }
    public string Name { get; set; }
}
