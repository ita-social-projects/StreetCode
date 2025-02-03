using FluentValidation;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Toponyms;
using Streetcode.BLL.SharedResource;

namespace Streetcode.BLL.Validators.Streetcode.Toponyms;

public class StreetcodeToponymValidator : AbstractValidator<StreetcodeToponymCreateUpdateDto>
{
    public const int StreetNameMaxLength = 150;
    public StreetcodeToponymValidator(IStringLocalizer<FailedToValidateSharedResource> localizer, IStringLocalizer<FieldNamesSharedResource> fieldLocalizer)
    {
        RuleFor(dto => dto.StreetName)
            .NotEmpty().WithMessage(localizer["CannotBeEmpty", fieldLocalizer["StreetName"]])
            .MaximumLength(StreetNameMaxLength).WithMessage(localizer["MaxLength", fieldLocalizer["StreetName"], StreetNameMaxLength]);
        RuleFor(dto => dto.ModelState)
            .IsInEnum().WithMessage(localizer["Invalid", fieldLocalizer["ModelState"]]);
    }
}