using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.DTO.Users.Expertise;
using Streetcode.DAL.Entities.Users.Expertise;
using System.ComponentModel.DataAnnotations;

namespace Streetcode.BLL.DTO.Users;

public class UserProfileDTO : BaseUserDTO
{
    public string Email { get; set; } = null!;
}