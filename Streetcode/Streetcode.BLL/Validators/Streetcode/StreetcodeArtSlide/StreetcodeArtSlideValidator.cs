using FluentValidation;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Media.Art;
using Streetcode.BLL.DTO.Toponyms;
using Streetcode.BLL.SharedResource;

namespace Streetcode.BLL.Validators.Streetcode.StreetcodeArtSlide;

public class StreetcodeArtSlideValidator : AbstractValidator<StreetcodeArtSlideCreateUpdateDTO>
{
    public StreetcodeArtSlideValidator(IStringLocalizer<FailedToValidateSharedResource> localizer, IStringLocalizer<FieldNamesSharedResource> fieldLocalizer)
    {
        RuleFor(dto => dto.StreetcodeArts)
            .NotEmpty().WithMessage(localizer["CannotBeEmpty", fieldLocalizer["StreetcodeArts"]]);
        RuleFor(dto => dto.Template)
            .IsInEnum().WithMessage(localizer["Invalid", fieldLocalizer["Template"]]);
        RuleFor(dto => dto.StreetcodeArts)
            .Must(arts => !HasDuplicateArtIds(arts))
            .WithMessage("Цей арт уже є в цьому слайді");
    }

    private bool HasDuplicateArtIds(IEnumerable<StreetcodeArtCreateUpdateDTO> arts)
    {
        return arts
            .GroupBy(a => a.ArtId)
            .Any(g => g.Count() > 1);
    }
}