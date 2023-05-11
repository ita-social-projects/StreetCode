using System.ComponentModel.DataAnnotations;
using Streetcode.DAL.Entities.Streetcode;

namespace Streetcode.DAL.Entities.Toponyms
{
    public class StreetcodeToponym
    {
        [Required]
        public int StreetcodeId { get; set; }

        [Required]
        public int ToponymId { get; set; }

        public StreetcodeContent? Streetcode { get; set; }

        public Toponym? Toponym { get; set; }
    }
}
