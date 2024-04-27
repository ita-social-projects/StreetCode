using Streetcode.BLL.DTO.Media.Images;

namespace Streetcode.BLL.DTO.Sources
{
    public class SourceLinkCategoryCreateDTO
    {
        public string Title { get; set; }
        public int ImageId { get; set; }
        public ImageDTO? Image { get; set; }
    }
}
