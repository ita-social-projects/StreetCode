using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Streetcode.DAL.Entities.Partners;

namespace Streetcode.DAL.Entities.Streetcode;

[Table("streetcode_partner", Schema = "streetcode")]
public class StreetcodePartner
{
    public bool IsSponsor { get; set; }

    [Required]
    public int StreetcodeId { get; set; }

    public StreetcodeContent? Streetcode { get; set; }

    [Required]
    public int PartnerId { get; set; }

    public Partner Partner { get; set; }
}