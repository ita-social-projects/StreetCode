namespace Streetcode.BLL.DTO.Streetcode.TextContent.Text;

public abstract class BaseTextDto
{
    public string Title { get; set; } = null!;
    public string TextContent { get; set; } = null!;
    public string? AdditionalText { get; set; }
}