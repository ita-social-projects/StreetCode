using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Entities.Streetcode;

namespace Streetcode.DAL.Entities.Sources;

[Table("source_link_categories", Schema = "sources")]
public class SourceLinkCategory
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [StringLength(23, ErrorMessage = "Title cannot be longer than 23 characters.")]
    [RegularExpression(@"\S", ErrorMessage = "Title cannot be empty or whitespace.")]
    public string? Title { get; set; }

    [Required]
    public int ImageId { get; set; }

    public Image? Image { get; set; }

    public List<StreetcodeContent> Streetcodes { get; set; } = new ();

    public List<StreetcodeCategoryContent> StreetcodeCategoryContents { get; set; } = new ();
}