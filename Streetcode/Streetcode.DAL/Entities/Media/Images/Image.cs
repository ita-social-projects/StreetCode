﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Streetcode.DAL.Entities.Partners;
using Streetcode.DAL.Entities.Sources;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Entities.News;
using Streetcode.DAL.Entities.Team;

namespace Streetcode.DAL.Entities.Media.Images;

[Table("images", Schema = "media")]
public class Image
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [MaxLength(100)]
    public string? Title { get; set; }

    [MaxLength(100)]
    public string? Alt { get; set; }

    [Required]
    public string BlobName { get; set; }

    [Required]
    public string MimeType { get; set; }

    public List<StreetcodeContent> Streetcodes { get; set; } = new ();

    public List<Fact> Facts { get; set; } = new ();

    public Art? Art { get; set; }

    public Partner? Partner { get; set; }

    public List<SourceLinkCategory> SourceLinkCategories { get; set; } = new ();

    public News.News News { get; set; }
    public TeamMember TeamMember { get; set; }
}
