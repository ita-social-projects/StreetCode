using FluentValidation;
using Streetcode.BLL.DTO.Media.Art;
using Streetcode.BLL.DTO.Toponyms;

namespace Streetcode.BLL.Validators.Streetcode.StreetcodeArtSlide;

public class StreetcodeArtSlideValidator : AbstractValidator<StreetcodeArtSlideCreateUpdateDTO>
{
    public StreetcodeArtSlideValidator()
    {
        RuleFor(dto => dto.StreetcodeArts)
            .NotEmpty().WithMessage("StreetcodeArts cannot be empty.");
        RuleFor(dto => dto.Template)
            .IsInEnum().WithMessage("Invalid template type.");
    }
}