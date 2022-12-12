using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFTask.Entities.Streetcode.TextContent;

[Table("texts", Schema = "streetcode")]
public class Text
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Title { get; set; }

    [Required]
    public string TextContent { get; set; }

    [Required]
    public int StreetcodeId { get; set; }

    public Streetcode? Streetcode { get; set; }
}