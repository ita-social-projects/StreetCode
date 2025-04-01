using FluentValidation;
using Streetcode.BLL.MediatR.Authentication.Login;

namespace Streetcode.BLL.Validators.Authentication;

public class LoginValidator : AbstractValidator<LoginQuery>
{
    public LoginValidator(BaseLoginValidator baseLoginValidator)
    {
        RuleFor(dto => dto.UserLogin).SetValidator(baseLoginValidator);
    }
}