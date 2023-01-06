using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Streetcode.DAL.Entities.Sources;

[Table("source_links", Schema = "sources")]
public class SourceLink
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [MaxLength(100)]
    public string? Title { get; set; }

    [Required]
    public string Url { get; set; }

    [Required]
    public int StreetcodeId { get; set; }

    public Streetcode.StreetcodeContent? Streetcode { get; set; }

    public List<SourceLinkSubCategory> SubCategories { get; set; } = new ();
}