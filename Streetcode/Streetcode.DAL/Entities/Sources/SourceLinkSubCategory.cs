using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Streetcode.DAL.Entities.Sources;

[Table("source_link_subcategories", Schema = "sources")]
public class SourceLinkSubCategory
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Title { get; set; }

    [Required]
    public int SourceLinkCategoryId { get; set; }

    public SourceLinkCategory? SourceLinkCategory { get; set; }

    public List<SourceLink> SourceLinks { get; set; } = new ();
}