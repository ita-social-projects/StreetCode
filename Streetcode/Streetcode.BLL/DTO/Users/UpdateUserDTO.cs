using System.ComponentModel.DataAnnotations;
using Streetcode.BLL.DTO.Users.Expertise;

namespace Streetcode.BLL.DTO.Users;

public class UpdateUserDTO
{
    [Required]
    public string Name { get; set; } = null!;
    [Required]
    public string Surname { get; set; } = null!;
    [Required]
    public string UserName { get; set; } = null!;
    public string? AboutYourself { get; set; }
    public int? AvatarId { get; set; } = null!;
    public List<ExpertiseDTO> Expertises { get; set; } = new();
    [Phone]
    public string PhoneNumber { get; set; } = null!;
}