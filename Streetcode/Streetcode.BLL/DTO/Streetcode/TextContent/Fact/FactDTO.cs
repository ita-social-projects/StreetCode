namespace Streetcode.BLL.DTO.Streetcode.TextContent.Fact;

public class FactDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public int ImageId { get; set; }
    public string FactContent { get; set; } = null!;
    public int Index { get; set; }
}
