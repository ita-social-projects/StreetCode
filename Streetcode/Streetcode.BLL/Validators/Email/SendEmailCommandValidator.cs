using FluentValidation;
using FluentValidation.Validators;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.MediatR.Email;
using Streetcode.BLL.SharedResource;

namespace Streetcode.BLL.Validators.Email;

public class SendEmailCommandValidator : AbstractValidator<SendEmailCommand>
{
    public const int ContentMaxLength = 500;
    public const int ContentMinLength = 1;
    public const int EmailMaxLength = 50;
    public SendEmailCommandValidator(
        IStringLocalizer<FieldNamesSharedResource> fieldLocalizer,
        IStringLocalizer<FailedToValidateSharedResource> localizer)
    {
        RuleFor(e => e.Email.From)
            .NotEmpty().WithMessage(localizer["CannotBeEmpty", fieldLocalizer["Email"]])
            .MaximumLength(EmailMaxLength).WithMessage(localizer["MaxLength", fieldLocalizer["Email"], EmailMaxLength])
            .EmailAddress(EmailValidationMode.Net4xRegex).WithMessage(localizer["EmailAddressFormat"]);

        // EmailValidationMode.Net4xRegex
        RuleFor(e => e.Email.Token)
            .NotEmpty().WithMessage(localizer["CannotBeEmpty", fieldLocalizer["CaptchaToken"]]);

        RuleFor(e => e.Email.Content)
            .Length(ContentMinLength, ContentMaxLength)
            .WithMessage(localizer["LengthMustBeInRange", fieldLocalizer["Content"], ContentMinLength, ContentMaxLength]);
    }
}