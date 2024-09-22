using System.ComponentModel.DataAnnotations;

namespace Streetcode.BLL.DTO.Email
{
    public class EmailDTO
    {
        [MaxLength(80)]
        public string From { get; set; }

        [MaxLength(80)]
        public string Source { get; set; } = null!;

        [Required]
        [StringLength(500, MinimumLength = 1)]
        public string Content { get; set; }

        [Required]
        public string Token { get; set; } = null!;
    }
}
