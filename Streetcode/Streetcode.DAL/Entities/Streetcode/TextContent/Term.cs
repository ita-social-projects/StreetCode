using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFTask.Entities.Streetcode.TextContent;

[Table("terms", Schema = "streetcode")]
public class Term
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required, MaxLength(50)]
    public string Title { get; set; }

    [Required]
    public string Description { get; set; }
}