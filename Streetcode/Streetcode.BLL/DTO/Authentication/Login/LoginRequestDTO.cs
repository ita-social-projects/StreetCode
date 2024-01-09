using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Streetcode.BLL.Constants.Authentication;
using Streetcode.BLL.Services.Authentication;

namespace Streetcode.BLL.DTO.Authentication.Login;

public class LoginRequestDTO
{
    [Required]
    [ValidEmail]
    [EmailAddress]
    [DefaultValue(AuthConstants.EMAIL)]
    public string Login { get; set; }

    [Required]
    [StrongPassword]
    [DefaultValue(AuthConstants.PASSWORD)]
    [MaxLength(30, ErrorMessage = "Password maximum length is 30")]
    [MinLength(14, ErrorMessage = "Password minimum length is 14")]
    public string Password { get; set; }
}
