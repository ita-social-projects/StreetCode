using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.DTO.Streetcode;

namespace Streetcode.BLL.DTO.Sources;

public class SourceLinkCategoryDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int ImageId { get; set; }
    public ImageDTO? Image { get; set; }
    public int StreetcodeId { get; set; }
    public StreetcodeDTO? Streetcode { get; set; }
    public IEnumerable<SourceLinkSubCategoryDTO> SubCategories { get; set; }
}