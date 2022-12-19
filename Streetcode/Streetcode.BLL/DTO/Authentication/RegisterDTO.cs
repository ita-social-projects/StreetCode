using System.ComponentModel.DataAnnotations;

namespace Streetcode.BLL.DTO.Authentication;

public class RegisterDTO
{
    [Required]
    [MaxLength(80)]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [RegularExpression(
        "(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{8,16}$",
        ErrorMessage = "Password must be complex (at least 1 digit, one uppercase letter, one lowercase letter)")]
    public string Password { get; set; }

    [Required]
    [MaxLength(100)]
    public string Username { get; set; }
}
