using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Streetcode.DAL.Entities.Streetcode.TextContent;

[Table("terms", Schema = "streetcode")]
public class Term
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string? Title { get; set; }

    [Required]
    [MaxLength(500)]
    public string? Description { get; set; }

    public List<RelatedTerm> RelatedTerms { get; set; } = new();
}