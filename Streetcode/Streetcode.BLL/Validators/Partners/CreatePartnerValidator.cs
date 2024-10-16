using FluentValidation;
using Streetcode.BLL.MediatR.Partners.Create;

namespace Streetcode.BLL.Validators.Partners;

public class CreatePartnerValidator : AbstractValidator<CreatePartnerQuery>
{
    public CreatePartnerValidator(BasePartnersValidator basePartnersValidator)
    {
        RuleFor(c => c.newPartner).SetValidator(basePartnersValidator);
    }
}