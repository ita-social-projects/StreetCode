using FluentValidation;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.Create;

namespace Streetcode.BLL.Validators.Streetcode;

public class CreateStreetcodeValidator : AbstractValidator<CreateStreetcodeCommand>
{
    public CreateStreetcodeValidator(BaseStreetcodeValidator baseStreetcodeValidator)
    {
        RuleFor(c => c.Streetcode).SetValidator(baseStreetcodeValidator);
    }
}