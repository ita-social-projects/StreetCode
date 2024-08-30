using FluentValidation;
using FluentValidation.Validators;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.MediatR.Email;
using Streetcode.BLL.SharedResource;

namespace Streetcode.BLL.Validators.Email;

public class SendEmailCommandValidator : AbstractValidator<SendEmailCommand>
{
    public SendEmailCommandValidator(
        IStringLocalizer<FieldNamesSharedResource> fieldLocalizer,
        IStringLocalizer<FailedToValidateSharedResource> localizer)
    {
        RuleFor(e => e.Email.From)
            .NotEmpty().WithMessage(localizer["CannotBeEmpty", fieldLocalizer["Email"]])
            .MaximumLength(80).WithMessage(localizer["MaxLength", fieldLocalizer["Email"]])
            .EmailAddress(EmailValidationMode.Net4xRegex).WithMessage(localizer["EmailAddressFormat"]);

        RuleFor(e => e.Email.Token)
            .NotEmpty().WithMessage(localizer["CannotBeEmpty", fieldLocalizer["Email"]]);

        RuleFor(e => e.Email.Content)
            .Length(1, 500).WithMessage(localizer["ContentLengthSendEmail"]);
    }
}