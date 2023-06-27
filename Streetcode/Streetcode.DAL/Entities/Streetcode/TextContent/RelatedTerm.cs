using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Streetcode.DAL.Entities.Streetcode.TextContent
{
    [Table("related_terms", Schema = "streetcode")]
    public class RelatedTerm
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Word { get; set; }
        [Required]
        public int TermId { get; set; }
        public Term? Term { get; set; }
    }
}
