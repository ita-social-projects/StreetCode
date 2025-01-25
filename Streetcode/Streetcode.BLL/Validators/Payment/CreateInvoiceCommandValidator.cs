using FluentValidation;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.MediatR.Payment;
using Streetcode.BLL.SharedResource;
using Streetcode.BLL.Validators.Common;

namespace Streetcode.BLL.Validators.Payment;

public class CreateInvoiceCommandValidator : AbstractValidator<CreateInvoiceCommand>
{
    private const int AmountGreaterThan = 0;

    public CreateInvoiceCommandValidator(
        IStringLocalizer<FieldNamesSharedResource> fieldLocalizer,
        IStringLocalizer<FailedToValidateSharedResource> localizer)
    {
        RuleFor(c => c.Payment.RedirectUrl)
            .NotEmpty().WithMessage(x => localizer["CannotBeEmpty", fieldLocalizer["RedirectUrl"]])
            .MustBeValidUrl().WithMessage(x => localizer["ValidUrl", fieldLocalizer["RedirectUrl"]]);

        RuleFor(c => c.Payment.Amount)
            .GreaterThan(AmountGreaterThan).WithMessage(x => localizer["GreaterThan", fieldLocalizer["Amount"], AmountGreaterThan]);
    }
}