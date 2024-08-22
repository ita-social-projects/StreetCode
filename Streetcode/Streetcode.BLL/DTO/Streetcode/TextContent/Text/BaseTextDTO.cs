namespace Streetcode.BLL.DTO.Streetcode.TextContent.Text;

public abstract class BaseTextDTO
{
    public string Title { get; set; }
    public string TextContent { get; set; }
    public string? AdditionalText { get; set; }
}