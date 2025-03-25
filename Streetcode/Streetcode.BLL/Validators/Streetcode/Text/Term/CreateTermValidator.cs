using FluentValidation;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.MediatR.Streetcode.Term.Create;
using Streetcode.BLL.SharedResource;

namespace Streetcode.BLL.Validators.Streetcode.Text.Term
{
    public class CreateTermValidator : AbstractValidator<CreateTermCommand>
    {
        public CreateTermValidator(
            BaseTermValidator baseTermValidator,
            IStringLocalizer<FailedToValidateSharedResource> localizer,
            IStringLocalizer<FieldNamesSharedResource> fieldLocalizer)
        {
            RuleFor(c => c.Term).SetValidator(baseTermValidator);
        }
    }
}
