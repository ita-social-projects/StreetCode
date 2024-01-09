using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Streetcode.BLL.Services.Authentication;

namespace Streetcode.BLL.DTO.Authentication;

public class RegisterRequestDTO
{
    [Required]
    [MaxLength(50)]
    [DefaultValue("John")]
    public string Name { get; set; }

    [Required]
    [MaxLength(50)]
    [DefaultValue("Doe")]
    public string Surname { get; set; }

    [Required]
    [EmailAddress]
    [ValidEmail]
    [DefaultValue("example@gmail.com")]
    public string Email { get; set; }

    [Required]
    [MaxLength(50, ErrorMessage = "UserName maximum length is 50")]
    [DefaultValue("John_Doe")]
    public string UserName { get; set; }

    [Required]
    [Display(Name = "Phone Number")]
    [Phone]
    [DefaultValue("+111-111-11-11")]
    public string PhoneNumber { get; set; }

    [Required]
    [MaxLength(30, ErrorMessage = "Password maximum length is 30")]
    [MinLength(14, ErrorMessage = "Password minimum length is 14")]
    [StrongPassword]
    [DefaultValue("")]
    public string Password { get; set; }

    [Required]
    [Display(Name = "Confirm password")]
    [MaxLength(30, ErrorMessage = "Password maximum length is 30")]
    [MinLength(14, ErrorMessage = "Password minimum length is 14")]
    [DefaultValue("")]
    [Compare(nameof(Password))]
    public string PasswordConfirmed { get; set; }
}
