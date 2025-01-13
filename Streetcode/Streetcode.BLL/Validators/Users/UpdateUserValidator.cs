using FluentValidation;
using Streetcode.BLL.MediatR.Users.Update;

namespace Streetcode.BLL.Validators.Users;

public class UpdateUserValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserValidator(BaseUserValidator baseUserValidator)
    {
        RuleFor(dto => dto.UserDto).SetValidator(baseUserValidator);
    }
}