using System.ComponentModel.DataAnnotations;
using Streetcode.BLL.Attributes.Authentication;

namespace Streetcode.BLL.DTO.Users.Password;

public class UpdateForgotPasswordDTO
{
    public string Token { get; set; }
    public string Username { get; set; }
    [StrongPassword]
    public string Password { get; set; } = null!;
    [Compare(nameof(Password))]
    [StrongPassword]
    public string ConfirmPassword { get; set; } = null!;
}