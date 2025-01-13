using FluentValidation;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Users;
using Streetcode.BLL.SharedResource;

namespace Streetcode.BLL.Validators.Users;

public class BaseUserValidator : AbstractValidator<UpdateUserDTO>
{
    public BaseUserValidator()
    {
        RuleFor(dto => dto.Expertises).Must(e => e.Count <= 3).WithMessage("Less than 3");

        RuleFor(dto => dto.AboutYourself).MaximumLength(500);

        RuleFor(dto => dto.Name).MinimumLength(3);

        RuleFor(dto => dto.Surname).MinimumLength(3);
    }
}