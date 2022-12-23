using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Streetcode.DAL.Entities.Streetcode;

namespace Streetcode.DAL.Entities.Partners;

[Table("streetcode_partner", Schema = "partners")]
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