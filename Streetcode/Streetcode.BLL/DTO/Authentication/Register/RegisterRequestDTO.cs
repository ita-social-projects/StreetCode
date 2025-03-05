using System.ComponentModel.DataAnnotations;
using Streetcode.BLL.Attributes.Authentication;

namespace Streetcode.BLL.DTO.Authentication.Register;

public class RegisterRequestDTO
{
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = null!;

    [Required]
    [MaxLength(50)]
    public string Surname { get; set; } = null!;

    [Required]
    [ValidEmail]
    public string Email { get; set; } = null!;

    [Required]
    [MaxLength(30, ErrorMessage = "Password maximum length is 30")]
    [StrongPassword]
    public string Password { get; set; } = null!;

    [Required]
    [Display(Name = "Confirm password")]
    [MaxLength(30, ErrorMessage = "Password maximum length is 30")]
    [Compare(nameof(Password))]
    public string PasswordConfirmation { get; set; } = null!;
}
