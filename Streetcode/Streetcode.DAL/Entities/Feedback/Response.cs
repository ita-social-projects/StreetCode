using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFTask.Entities.Feedback;

[Table("responses", Schema = "feedback")]
public class Response
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [MaxLength(50)]
    public string? FirstName { get; set; }
    
    [Required, MaxLength(50)]
    [EmailAddress]
    public string Email { get; set; }

    [Column(TypeName = "text")]
    public string? Description { get; set; }
}