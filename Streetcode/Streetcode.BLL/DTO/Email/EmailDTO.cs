using System.ComponentModel.DataAnnotations;

namespace Streetcode.BLL.DTO.Email
{
    public class EmailDTO
    {
        [MaxLength(80)]
        public string From { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 1)]
        public string Content { get; set; }
    }
}
