using FluentValidation;
using FluentValidation.Validators;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.MediatR.Email;
using Streetcode.BLL.SharedResource;

namespace Streetcode.BLL.Validators.Email;

public class SendEmailCommandValidator : AbstractValidator<SendEmailCommand>
{
    public SendEmailCommandValidator(IStringLocalizer<FailedToValidateSharedResource> localizer)
    {
        RuleFor(e => e.Email.From)
            .NotEmpty().WithMessage(localizer["EmailCannotBeEmpty"])
            .MaximumLength(80).WithMessage(localizer["MaxLengthEmail"])
            .EmailAddress(EmailValidationMode.Net4xRegex).WithMessage(localizer["EmailAddressFormat"]);

        RuleFor(e => e.Email.Token)
            .NotNull()
            .NotEmpty();

        RuleFor(e => e.Email.Content)
            .Length(1, 500).WithMessage(localizer["ContentLengthSendEmail"]);
    }
}