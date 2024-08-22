using FluentValidation;
using Streetcode.BLL.DTO.Media.Art;
using Streetcode.BLL.DTO.Toponyms;

namespace Streetcode.BLL.Validators.Streetcode.StreetcodeArtSlide;

public class StreetcodeArtSlideValidator : AbstractValidator<StreetcodeArtSlideCreateUpdateDTO>
{
    public StreetcodeArtSlideValidator(StreetcodeArtCreateUpdateDTOValidator streetcodeArtCreateUpdateDtoValidator)
    {
        RuleFor(dto => dto.Index)
            .InclusiveBetween(BaseStreetcodeValidator.IndexMinValue, BaseStreetcodeValidator.IndexMaxValue)
            .WithMessage($"Index should be between {BaseStreetcodeValidator.IndexMinValue} and {BaseStreetcodeValidator.IndexMaxValue}");

        RuleFor(dto => dto.StreetcodeArts)
            .NotEmpty().WithMessage("StreetcodeArts cannot be empty.")
            .ForEach(art => art.SetValidator(streetcodeArtCreateUpdateDtoValidator));

        RuleFor(dto => dto.Template)
            .IsInEnum().WithMessage("Invalid template type.");
    }
}