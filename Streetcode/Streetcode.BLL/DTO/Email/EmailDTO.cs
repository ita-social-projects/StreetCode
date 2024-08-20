using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Streetcode.BLL.DTO.Email
{
    [ValidateNever]
    public class EmailDTO
    {
        [MaxLength(80)]
        public string From { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 1)]
        public string Content { get; set; }

        [Required]
        public string Token { get; set; } = null!;
    }
}
