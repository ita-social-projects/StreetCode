using System.ComponentModel.DataAnnotations;
using Streetcode.BLL.Attributes.Authentication;

namespace Streetcode.BLL.DTO.Authentication.Register;

public class RegisterRequestDTO
{
    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string PasswordConfirmation { get; set; } = null!;
}
