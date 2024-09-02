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
    public string Name { get; set; } = null!;

    [Required]
    [MaxLength(50)]
    [DefaultValue(AuthConstants.Surname)]
    public string Surname { get; set; } = null!;

    [Required]
    [EmailAddress]
    [DefaultValue(AuthConstants.Email)]
    [RegularExpression(@"[^\.\-_](?:[a-z0-9!#$%&'*+\/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+\/=?^_`{|}~-]+)*|""(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*"")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9]))\.){3}(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9])|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])", ErrorMessage = "The Email field doesn't contain a valid email address")]
    public string Email { get; set; } = null!;

    [Required]
    [MaxLength(50, ErrorMessage = "UserName maximum length is 50")]
    [DefaultValue(AuthConstants.Username)]
    public string UserName { get; set; } = null!;

    [Display(Name = "Phone Number")]
    [Phone]
    [DefaultValue(AuthConstants.PhoneNumber)]
    public string PhoneNumber { get; set; } = null!;

    [Required]
    [MaxLength(30, ErrorMessage = "Password maximum length is 30")]
    [StrongPassword]
    [DefaultValue(AuthConstants.Password)]
    public string Password { get; set; } = null!;

    [Required]
    [Display(Name = "Confirm password")]
    [MaxLength(30, ErrorMessage = "Password maximum length is 30")]
    [DefaultValue("")]
    [Compare(nameof(Password))]
    public string PasswordConfirmation { get; set; } = null!;
}
