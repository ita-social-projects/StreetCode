using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Streetcode.BLL.Attributes.Authentication;
using Streetcode.BLL.Constants.Authentication;

namespace Streetcode.BLL.DTO.Authentication.Login;

public class LoginRequestDTO
{
    [Required]
    [ValidEmail]
    [EmailAddress]
    [DefaultValue(AuthConstants.Email)]
    public string Login { get; set; }

    [Required]
    [StrongPassword]
    [DefaultValue(AuthConstants.Password)]
    [MaxLength(30, ErrorMessage = "Password maximum length is 30")]
    public string Password { get; set; }
}
