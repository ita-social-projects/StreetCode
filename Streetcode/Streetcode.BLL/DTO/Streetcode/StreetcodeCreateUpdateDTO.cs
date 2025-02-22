using Streetcode.BLL.DTO.Media.Art;
using Streetcode.BLL.DTO.Media.Create;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.DTO.Timeline.Update;
using Streetcode.BLL.DTO.Toponyms;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.DTO.Streetcode;

public abstract class StreetcodeCreateUpdateDTO
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public int Index { get; set; }

    public string? Teaser { get; set; }

    public string DateString { get; set; } = null!;

    public string? Alias { get; set; }

    public StreetcodeStatus Status { get; set; }

    public StreetcodeType StreetcodeType { get; set; }

    public string Title { get; set; } = null!;

    public string TransliterationUrl { get; set; } = null!;

    public DateTime EventStartOrPersonBirthDate { get; set; }

    public DateTime? EventEndOrPersonDeathDate { get; set; }

    public IEnumerable<StreetcodeToponymCreateUpdateDTO>? Toponyms { get; set; } = new List<StreetcodeToponymCreateUpdateDTO>();

    public IEnumerable<TimelineItemCreateUpdateDTO>? TimelineItems { get; set; } = new List<TimelineItemCreateUpdateDTO>();

    public IEnumerable<ImageDetailsDto> ImagesDetails { get; set; } = new List<ImageDetailsDto>();

    public IEnumerable<StreetcodeArtSlideCreateUpdateDTO>? StreetcodeArtSlides { get; set; } = new List<StreetcodeArtSlideCreateUpdateDTO>();

    public List<ArtCreateUpdateDTO>? Arts { get; set; } = new List<ArtCreateUpdateDTO>();
}