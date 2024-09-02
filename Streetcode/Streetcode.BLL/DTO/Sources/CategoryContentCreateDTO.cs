namespace Streetcode.BLL.DTO.Sources
{
  public class CategoryContentCreateDTO
  {
    public int? Id { get; set; }
    public int SourceLinkCategoryId { get; set; }
    public string Text { get; set; } = null!;
    public int StreetcodeId { get; set; }
  }
}
