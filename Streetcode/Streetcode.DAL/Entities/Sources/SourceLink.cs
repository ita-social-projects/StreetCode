using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFTask.Entities.Sources;

[Table("source_links", Schema = "sources")]
public class SourceLink
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required, MaxLength(100)] 
    public string Title { get; set; }
    
    public string? Url { get; set; }

    [Required]
    public int StreetcodeId { get; set; }

    public Streetcode.Streetcode? Streetcode { get; set; }

    public List<SourceLinkCategory> Categories { get; set; } = new();
}

