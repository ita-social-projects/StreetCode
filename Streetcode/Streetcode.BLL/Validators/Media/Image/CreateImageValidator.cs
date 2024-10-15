using FluentValidation;
using Streetcode.BLL.MediatR.Media.Image.Create;

namespace Streetcode.BLL.Validators.Media.Image;

public class CreateImageValidator : AbstractValidator<CreateImageCommand>
{
    public CreateImageValidator(BaseImageValidator baseImageValidator)
    {
        RuleFor(c => c.Image).SetValidator(baseImageValidator);
    }
}