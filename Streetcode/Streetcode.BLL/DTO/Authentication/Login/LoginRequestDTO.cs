using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Streetcode.BLL.Constants.Authentication;
using Streetcode.BLL.Attributes.Authentication;

namespace Streetcode.BLL.DTO.Authentication.Login;

public class LoginRequestDTO
{
    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string CaptchaToken { get; set; } = null!;
}
