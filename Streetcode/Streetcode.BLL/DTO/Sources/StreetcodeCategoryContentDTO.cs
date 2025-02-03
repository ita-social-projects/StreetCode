using System.ComponentModel.DataAnnotations;

namespace Streetcode.BLL.DTO.Sources
{
    public class StreetcodeCategoryContentDto
    {
        [Required]
        [MaxLength(15000)]
        public string Text { get; set; } = null!;

        [Required]
        public int SourceLinkCategoryId { get; set; }

        [Required]
        public int StreetcodeId { get; set; }
    }
}
