using System.ComponentModel.DataAnnotations;
using Streetcode.BLL.Services.Authentication;

namespace Streetcode.BLL.DTO.Authentication;

public class RegisterRequestDTO
{
    [Required]
    [MaxLength(50)]
    public string Name { get; set; }

    [Required]
    [MaxLength(50)]
    public string Surname { get; set; }

    [Required]
    [EmailAddress]
    [ValidEmail]
    public string Email { get; set; }

    [Required]
    [MaxLength(50, ErrorMessage = "UserName maximum length is 50")]
    public string UserName { get; set; }

    [Required]
    [Display(Name = "Phone Number")]
    [Phone]
    public string PhoneNumber { get; set; }

    [Required]
    [MaxLength(30, ErrorMessage = "Password maximum length is 30")]
    [MinLength(14, ErrorMessage = "Password minimum length is 14")]
    [StrongPassword]
    public string Password { get; set; }

    [Required]
    [Display(Name = "Confirm password")]
    [MaxLength(30, ErrorMessage = "Password maximum length is 30")]
    [MinLength(14, ErrorMessage = "Password minimum length is 14")]
    [Compare(nameof(Password))]
    public string PasswordConfirmed { get; set; }
}
