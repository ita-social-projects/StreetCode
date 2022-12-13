using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EFTask.Entities.Partners;

namespace Streetcode.DAL.Entities.Partners;

[Table("streetcode_partners", Schema = "partner_sponsors")]
public class StreetcodePartner
{
    public bool IsSponsor { get; set; }

    [Required]
    public int StreetcodeId { get; set; }

    public Streetcode.Streetcode? StreetCode { get; set; }

    [Required]
    public int PartnerId { get; set; }

    public Partner Partner { get; set; }
}