namespace Streetcode.BLL.DTO.Streetcode.TextContent.Text;

public class TextDTO
{
  public int Id { get; set; }
  public string Title { get; set; }
  public string TextContent { get; set; }
  public int StreetcodeId { get; set; }
  public string? AdditionalText { get; set; }
}
