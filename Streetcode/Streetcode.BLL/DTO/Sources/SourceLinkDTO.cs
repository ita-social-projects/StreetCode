using Streetcode.BLL.DTO.AdditionalContent;

namespace Streetcode.BLL.DTO.Sources;

public class SourceLinkDTO
{
    public int Id { get; set; }
    public UrlDTO Url { get; set; }
    public IEnumerable<SourceLinkSubCategoryDTO> SubCategories { get; set; }
}