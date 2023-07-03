namespace Streetcode.BLL.DTO.Streetcode.TextContent;

public class TextDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string TextContent { get; set; }
    public int StreetcodeId { get; set; }
    public string? AdditionalText { get; set; }
}