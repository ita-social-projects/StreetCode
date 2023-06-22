using Streetcode.BLL.DTO.Media.Images;

namespace Streetcode.BLL.DTO.Media.Create
{
    public class ArtCreateUpdateDTO
    {
        public int Id { get; set; }
        public ImageDTO Image { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
    }
}
