using FluentValidation;
using Streetcode.BLL.MediatR.Authentication.Register;

namespace Streetcode.BLL.Validators.Authentication;

public class RegisterValidator : AbstractValidator<RegisterQuery>
{
    public RegisterValidator(BaseRegisterValidator baseRegisterValidator)
    {
        RuleFor(dto => dto.registerRequestDTO).SetValidator(baseRegisterValidator);
    }
}