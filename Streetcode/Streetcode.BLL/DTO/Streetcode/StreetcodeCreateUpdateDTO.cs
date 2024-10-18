using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
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
    public string? Teaser { get; set; } // in the requirements it's not specified whether it is a mandatory property or not
    public string DateString { get; set; } = null!; // date in a pretty format
    public string? Alias { get; set; } // this is "Короткий опис (для зв'язків історії)"
    public StreetcodeStatus Status { get; set; } // passed as a number
    public StreetcodeType StreetcodeType { get; set; } // an event or a person
    public string Title { get; set; } = null!; // this is "Назва стріткоду"
    public string TransliterationUrl { get; set; } = null!; // this is "URL"
    public DateTime EventStartOrPersonBirthDate { get; set; }
    public DateTime? EventEndOrPersonDeathDate { get; set; }
    public IEnumerable<StreetcodeToponymCreateUpdateDTO> Toponyms { get; set; } = null!;
    public IEnumerable<TimelineItemCreateUpdateDTO> TimelineItems { get; set; } = null!;
    public IEnumerable<ImageDetailsDto>? ImagesDetails { get; set; } // strange behaviour, a random number is passed as 'Alt' property
    public IEnumerable<StreetcodeArtSlideCreateUpdateDTO> StreetcodeArtSlides { get; set; } = null!;
    public List<ArtCreateUpdateDTO> Arts { get; set; } = null!;
}