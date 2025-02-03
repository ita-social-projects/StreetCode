using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Streetcode.BLL.DTO.Users.Password;

public class UpdateForgotPasswordDto
{
    public string Token { get; set; }
    public string Username { get; set; }
    public string Password { get; set; } = null!;
    [Compare(nameof(Password))]
    public string ConfirmPassword { get; set; } = null!;
}