using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Streetcode.BLL.Constants.Authentication;
using Streetcode.BLL.Attributes.Authentication;

namespace Streetcode.BLL.DTO.Authentication.Login;

public class LoginRequestDTO
{
    [Required]
    [ValidEmail]
    [DefaultValue(AuthConstants.Email)]
    public string Login { get; set; } = null!;

    [Required]
    [DefaultValue(AuthConstants.Password)]
    [MaxLength(30, ErrorMessage = "Password maximum length is 30")]
    public string Password { get; set; } = null!;

    public string CaptchaToken { get; set; } = null!;
}
