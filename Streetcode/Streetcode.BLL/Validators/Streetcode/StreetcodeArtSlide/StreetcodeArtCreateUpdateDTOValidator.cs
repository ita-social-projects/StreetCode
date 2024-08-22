using FluentValidation;
using Streetcode.BLL.DTO.Media.Art;

namespace Streetcode.BLL.Validators.Streetcode.StreetcodeArtSlide;

public class StreetcodeArtCreateUpdateDTOValidator : AbstractValidator<StreetcodeArtCreateUpdateDTO>
{
    public StreetcodeArtCreateUpdateDTOValidator()
    {
        RuleFor(dto => dto.Index)
            .InclusiveBetween(BaseStreetcodeValidator.IndexMinValue, BaseStreetcodeValidator.IndexMinValue)
            .WithMessage($"Index should be between {BaseStreetcodeValidator.IndexMinValue} and {BaseStreetcodeValidator.IndexMinValue}");
    }
}