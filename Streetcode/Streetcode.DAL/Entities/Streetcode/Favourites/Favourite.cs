using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Streetcode.DAL.Entities.Users;

namespace Streetcode.DAL.Entities.Streetcode.Favourites
{
    [Table("favourites", Schema = "streetcode")]
    public class Favourite
    {
        [Required]
        public int StreetcodeId { get; set; }
        [Required]
        public string UserId { get; set; } = string.Empty;
        public StreetcodeContent Streetcode { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}
