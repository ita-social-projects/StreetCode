using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Users;

namespace Streetcode.DAL.Entities.Users.Favourites
{
    [Table("favourite_streetcodes", Schema = "user")]
    public class Favourites
    {
        [Required]
        public int StreetcodeId { get; set; }
        [Required]
        public string UserId { get; set; }
        public StreetcodeContent? Streetcode { get; set; }
        public User? User { get; set; }
    }
}
