using Streetcode.BLL.DTO.Media.Images;

namespace Streetcode.BLL.DTO.News
{
    public class NewsDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Text { get; set; } = null!;
        public int ImageId { get; set; }
        public string URL { get; set; } = null!;
        public ImageDto Image { get; set; } = null!;
        public DateTime CreationDate { get; set; }
    }
}
