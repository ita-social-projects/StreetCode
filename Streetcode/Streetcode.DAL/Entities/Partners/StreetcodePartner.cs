using System.ComponentModel.DataAnnotations;
using Streetcode.DAL.Entities.Streetcode;

namespace Streetcode.DAL.Entities.Partners
{
    public class StreetcodePartner
    {
        [Required]
        public int StreetcodeId { get; set; }

        [Required]
        public int PartnerId { get; set; }

        public StreetcodeContent? Streetcode { get; set; }

        public Partner? Partner { get; set; }
    }
}
