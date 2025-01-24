using System.ComponentModel.DataAnnotations;
using Streetcode.BLL.DTO.Users.Expertise;

namespace Streetcode.BLL.DTO.Users;

public class UpdateUserDTO
{
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string? AboutYourself { get; set; }
    public int? AvatarId { get; set; } = null!;
    public List<ExpertiseDTO> Expertises { get; set; } = new();
    [Phone]
    public string PhoneNumber { get; set; } = null!;
    public string Email { get; set; } = null!;
}