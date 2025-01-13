using Streetcode.BLL.DTO.Users.Expertise;
using System.ComponentModel.DataAnnotations;

namespace Streetcode.BLL.DTO.Users
{
    public class UserDTO : BaseUserDTO
    {
        public string Email { get; set; } = null!;
        public string Role { get; set; } = null!;
    }
}
