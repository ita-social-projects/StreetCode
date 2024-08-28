using FluentValidation;
using Streetcode.BLL.MediatR.Partners.Update;

namespace Streetcode.BLL.Validators.Partners;

public class UpdatePartnerValidator : AbstractValidator<UpdatePartnerQuery>
{
    public UpdatePartnerValidator(BasePartnersValidator basePartnersValidator)
    {
        RuleFor(c => c.Partner).SetValidator(basePartnersValidator);
    }
}