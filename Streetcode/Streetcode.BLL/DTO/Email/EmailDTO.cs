using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Streetcode.BLL.DTO.Email
{
    [ValidateNever]
    public class EmailDTO
    {
        [MaxLength(80)]
        public string From { get; set; } = null!;

        [MaxLength(80)]
        public string Source { get; set; } = null!;

        [Required]
        [StringLength(500, MinimumLength = 1)]
        public string Content { get; set; } = null!;

        [Required]
        public string Token { get; set; } = null!;
    }
}
