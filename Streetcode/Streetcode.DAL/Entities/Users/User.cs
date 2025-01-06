using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Streetcode.DAL.Entities.Media.Images;

namespace Streetcode.DAL.Entities.Users
{
    public class User : IdentityUser
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = null!;
        [Required]
        [MaxLength(50)]
        public string Surname { get; set; } = null!;
        public string? RefreshToken { get; set; }
        public string? AboutYourself { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }
        public Image? Avatar { get; set; }
        public List<Expertise.Expertise> Expertises { get; set; } = new ();
    }
}
