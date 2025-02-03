using FluentValidation;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.AdditionalContent.Tag;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.Validators.AdditionalContent.Tag;

public class BaseTagValidator : AbstractValidator<CreateUpdateTagDto>
{
    public const int TitleMaxLength = 50;
    public BaseTagValidator(
        IStringLocalizer<FailedToValidateSharedResource> localizer,
        IStringLocalizer<FieldNamesSharedResource> fieldLocalizer)
    {
        RuleFor(t => t.Title)
            .NotEmpty().WithMessage(x => localizer["CannotBeEmpty", fieldLocalizer["Title"]])
            .MaximumLength(TitleMaxLength)
            .WithMessage(x => localizer["MaxLength", fieldLocalizer["Title"], TitleMaxLength]);
    }
}