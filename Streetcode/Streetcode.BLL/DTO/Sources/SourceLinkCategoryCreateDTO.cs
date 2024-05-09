using Streetcode.BLL.DTO.Media.Images;

namespace Streetcode.BLL.DTO.Sources
{
    public class SourceLinkCategoryCreateDTO : SourceLinkCreateUpdateCategoryDTO
    {
        public ImageDTO? Image { get; set; }
    }
}
