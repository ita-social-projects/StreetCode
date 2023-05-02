using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Streetcode.DAL.Entities.Streetcode;

namespace Streetcode.DAL.Entities.AdditionalContent
{
    [Table("streetcode_tag_index", Schema = "add_content")]
    public class StreetcodeTagIndex
    {
        [Required]
        public int StreetcodeId { get; set; }

        [Required]
        public int TagId { get; set; }

        [Required]
        public bool IsVisible { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Index { get; set; }

        public StreetcodeContent? Streetcode { get; set; }

        public Tag? Tag { get; set; }
    }
}
