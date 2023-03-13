using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Streetcode.DAL.Entities.Streetcode.TextContent
{
    [Table("relatedTerms", Schema = "streetcode")]
    public class RelatedTerm
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Word { get; set; }
        [Required]
        public int TermId { get; set; }
        public Term Term { get; set; }
    }
}
