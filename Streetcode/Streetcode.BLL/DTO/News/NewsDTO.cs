using System.ComponentModel.DataAnnotations;
using Streetcode.BLL.DTO.Media.Images;

namespace Streetcode.BLL.DTO.News
{
    public class NewsDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Text { get; set; } = null!;
        public int? ImageId { get; set; }
        public string URL { get; set; } = null!;
        public ImageDTO Image { get; set; } = null!;
        public DateTime CreationDate { get; set; }
    }
}
