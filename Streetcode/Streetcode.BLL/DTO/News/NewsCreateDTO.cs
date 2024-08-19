using System.ComponentModel.DataAnnotations;
using Streetcode.BLL.DTO.Media.Images;

namespace Streetcode.BLL.DTO.News
{
    public class NewsCreateDTO
    {
        [Required(AllowEmptyStrings = false)]
        [StringLength(100, ErrorMessage = "Max Length is 100")]
        public string Title { get; set; }
        [Required(AllowEmptyStrings = false)]
        [StringLength(15000, ErrorMessage = "Max Length is 15000")]
        public string Text { get; set; }
        public int ImageId { get; set; }
        [Required(AllowEmptyStrings = false)]
        [StringLength(200, ErrorMessage = "Max Length is 200")]
        public string URL { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
