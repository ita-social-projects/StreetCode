using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Streetcode.Favourites;

namespace Streetcode.DAL.Entities.Users
{
    public class User : IdentityUser
    {
        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        public new string UserName
        {
            get => base.UserName ?? string.Empty;
            set => base.UserName = value ?? throw new ArgumentNullException(nameof(UserName), "Username cannot be null");
        }

        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        public string Name { get; set; } = null!;
        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        public string Surname { get; set; } = null!;
        public string? RefreshToken { get; set; }
        [MaxLength(500)]
        public string? AboutYourself { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }
        public int? AvatarId { get; set; }
        public Image? Avatar { get; set; }
        public List<Expertise.Expertise> Expertises { get; set; } = new();
        public List<StreetcodeContent>? StreetcodeContent { get; set; } = new();
        public List<StreetcodeContent>? StreetcodeFavourites { get; set; } = new();
    }
}
