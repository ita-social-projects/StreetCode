using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Streetcode.DAL.Entities.Media.Images;

namespace Streetcode.DAL.Entities.Sources;

[Table("source_link_categories", Schema = "sources")]
public class SourceLinkCategory
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Title { get; set; }

    [Required]
    public int ImageId { get; set; }

    public Image? Image { get; set; }

    public List<SourceLink> SourceLinks { get; set; } = new ();
}