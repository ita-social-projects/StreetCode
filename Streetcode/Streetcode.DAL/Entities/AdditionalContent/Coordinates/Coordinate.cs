using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Streetcode.DAL.Entities.AdditionalContent.Coordinates;

[Table("coordinates", Schema = "add_content")]
public class Coordinate
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,4)")]
    public decimal Latitude { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,4)")]
    public decimal Longtitude { get; set; }
}