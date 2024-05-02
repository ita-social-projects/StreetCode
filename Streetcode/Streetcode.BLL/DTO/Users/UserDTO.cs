using System.ComponentModel.DataAnnotations;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.DTO.Users
{
    public class UserDTO
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        [MaxLength(50)]
        public string Surname { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MaxLength(20)]
        public string UserName { get; set; }
        public string Role { get; set; }
    }
}
