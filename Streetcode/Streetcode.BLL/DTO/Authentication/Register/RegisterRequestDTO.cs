using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Streetcode.BLL.Attributes.Authentication;
using Streetcode.BLL.Constants.Authentication;

namespace Streetcode.BLL.DTO.Authentication.Register;

public class RegisterRequestDTO
{
    [Required]
    [MaxLength(50)]
    [DefaultValue(AuthConstants.Name)]
    public string Name { get; set; }

    [Required]
    [MaxLength(50)]
    [DefaultValue(AuthConstants.Surname)]
    public string Surname { get; set; }

    [Required]
    [EmailAddress]
    [ValidEmail]
    [DefaultValue(AuthConstants.Email)]
    public string Email { get; set; }

    [Required]
    [MaxLength(50, ErrorMessage = "UserName maximum length is 50")]
    [DefaultValue(AuthConstants.Username)]
    public string UserName { get; set; }

    [Display(Name = "Phone Number")]
    [Phone]
    [DefaultValue(AuthConstants.PhoneNumber)]
    public string? PhoneNumber { get; set; }

    [Required]
    [MaxLength(30, ErrorMessage = "Password maximum length is 30")]
    [StrongPassword]
    [DefaultValue(AuthConstants.Password)]
    public string Password { get; set; }

    [Required]
    [Display(Name = "Confirm password")]
    [MaxLength(30, ErrorMessage = "Password maximum length is 30")]
    [DefaultValue("")]
    [Compare(nameof(Password))]
    public string PasswordConfirmation { get; set; }
}
