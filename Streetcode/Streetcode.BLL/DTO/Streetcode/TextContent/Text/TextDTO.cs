namespace Streetcode.BLL.DTO.Streetcode.TextContent.Text;

public class TextDTO
{
  public int Id { get; set; }
  public string Title { get; set; } = null!;
  public string TextContent { get; set; } = null!;
  public int StreetcodeId { get; set; }
  public string AdditionalText { get; set; } = null!;
}
