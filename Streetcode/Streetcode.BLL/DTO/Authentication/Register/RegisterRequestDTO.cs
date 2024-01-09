using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Streetcode.BLL.Constants.Authentication;
using Streetcode.BLL.Services.Authentication;

namespace Streetcode.BLL.DTO.Authentication.Register;

public class RegisterRequestDTO
{
    [Required]
    [MaxLength(50)]
    [DefaultValue(AuthConstants.NAME)]
    public string Name { get; set; }

    [Required]
    [MaxLength(50)]
    [DefaultValue(AuthConstants.SURNAME)]
    public string Surname { get; set; }

    [Required]
    [EmailAddress]
    [ValidEmail]
    [DefaultValue(AuthConstants.EMAIL)]
    public string Email { get; set; }

    [Required]
    [MaxLength(50, ErrorMessage = "UserName maximum length is 50")]
    [DefaultValue(AuthConstants.USERNAME)]
    public string UserName { get; set; }

    [Required]
    [Display(Name = "Phone Number")]
    [Phone]
    [DefaultValue(AuthConstants.PHONENUMBER)]
    public string PhoneNumber { get; set; }

    [Required]
    [MaxLength(30, ErrorMessage = "Password maximum length is 30")]
    [MinLength(14, ErrorMessage = "Password minimum length is 14")]
    [StrongPassword]
    [DefaultValue(AuthConstants.PASSWORD)]
    public string Password { get; set; }

    [Required]
    [Display(Name = "Confirm password")]
    [MaxLength(30, ErrorMessage = "Password maximum length is 30")]
    [MinLength(14, ErrorMessage = "Password minimum length is 14")]
    [DefaultValue("")]
    [Compare(nameof(Password))]
    public string PasswordConfirmed { get; set; }
}
