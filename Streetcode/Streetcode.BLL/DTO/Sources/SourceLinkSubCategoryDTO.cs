namespace Streetcode.BLL.DTO.Sources;

public class SourceLinkSubCategoryDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int SourceLinkCategoryId { get; set; }
    public SourceLinkCategoryDTO? SourceLinkCategory { get; set; }
    public IEnumerable<SourceLinkDTO> SourceLinks { get; set; }
}