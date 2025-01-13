using Streetcode.BLL.DTO.Users.Expertise;
using System.ComponentModel.DataAnnotations;

namespace Streetcode.BLL.DTO.Users;

public class UpdateUserDTO
{
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = null!;
    [Required]
    [MaxLength(50)]
    public string Surname { get; set; } = null!;
    [Required]
    [MaxLength(20)]
    public string UserName { get; set; } = null!;
    public string? AboutYourself { get; set; }
    public int? AvatarId { get; set; } = null!;
    public List<ExpertiseDTO> Expertises { get; set; } = new();
    [Phone]
    public string PhoneNumber { get; set; } = null!;
}