using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EFTask.Entities.Media.Images;

namespace EFTask.Entities.Streetcode.TextContent;

[Table("facts", Schema = "streetcode")]
public class Fact
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Title { get; set; }

    [Required]
    public string FactContent { get; set; }
    
    public int? ImageId { get; set; }
        
    public Image? Image { get; set; }

    public List<Streetcode> Streetcodes { get; set; } = new();
}
