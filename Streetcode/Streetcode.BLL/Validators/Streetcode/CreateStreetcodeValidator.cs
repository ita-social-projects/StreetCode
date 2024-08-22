using FluentValidation;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.Create;
using Streetcode.BLL.Validators.Streetcode.Text;

namespace Streetcode.BLL.Validators.Streetcode;

public class CreateStreetcodeValidator : AbstractValidator<CreateStreetcodeCommand>
{
    public CreateStreetcodeValidator(BaseStreetcodeValidator baseStreetcodeValidator, BaseTextValidator baseTextValidator)
    {
        RuleFor(c => c.Streetcode).SetValidator(baseStreetcodeValidator);
        RuleFor(c => c.Streetcode.Text).SetValidator(baseTextValidator);
    }
}