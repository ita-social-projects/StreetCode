using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streetcode.BLL.DTO.Users
{
    public class UserRegisterDTO
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        [MaxLength(50)]
        public string Surname { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MaxLength(20, ErrorMessage = "UserName maximum length is 20")]
        public string UserName { get; set; }
        [Required]
        [MaxLength(20, ErrorMessage = "Password maximum length is 20")]
        [MinLength(14, ErrorMessage = "Password minimum length is 14")]
        public string Password { get; set; }
        [Required]
        [Display(Name = "Confirm password")]
        [MaxLength(20, ErrorMessage = "Password maximum length is 20")]
        [MinLength(14, ErrorMessage = "Password minimum length is 14")]
        public string PasswordConfirmed { get; set; }
    }
}
