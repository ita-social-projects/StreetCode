using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Entities.Streetcode;

namespace Streetcode.DAL.Entities.Partners;

[Table("partners", Schema = "partners")]
public class Partner
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(255)]
    public string Title { get; set; }

    [Required]
    public int LogoId { get; set; }

    [Required]
    public bool IsKeyPartner { get; set; }

    [Required]
    public bool IsVisibleEverywhere { get; set; }

    [MaxLength(255)]
    public string? TargetUrl { get; set; }

    [MaxLength(255)]
    public string? UrlTitle { get; set; }
    [MaxLength(600)]
    public string? Description { get; set; }

    public Image? Logo { get; set; }

    public List<PartnerSourceLink> PartnerSourceLinks { get; set; } = new ();

    public List<StreetcodeContent> Streetcodes { get; set; } = new ();
}