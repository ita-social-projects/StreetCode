using FluentValidation;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Authentication.Login;
using Streetcode.BLL.SharedResource;

namespace Streetcode.BLL.Validators.Authentication;

public class BaseLoginValidator : AbstractValidator<LoginRequestDTO>
{
    public BaseLoginValidator(
        IStringLocalizer<FailedToValidateSharedResource> localizer,
        IStringLocalizer<FieldNamesSharedResource> fieldLocalizer)
    {
        RuleFor(x => x.Login)
            .Matches(@"^(?!.*\.\.)[a-zA-Z0-9_%+-]+(?:\.[a-zA-Z0-9_%+-]+)*@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$").WithMessage(localizer["EmailAddressFormat"])
            .NotEmpty().WithMessage(localizer["CannotBeEmpty", fieldLocalizer["Email"]]);

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(localizer["CannotBeEmpty", fieldLocalizer["Password"]]);

        RuleFor(x => x.CaptchaToken)
            .NotEmpty().WithMessage(localizer["CannotBeEmpty", fieldLocalizer["CaptchaToken"]]);
    }
}