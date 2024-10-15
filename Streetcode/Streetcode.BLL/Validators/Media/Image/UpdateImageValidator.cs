using FluentValidation;
using Streetcode.BLL.MediatR.Media.Image.Update;

namespace Streetcode.BLL.Validators.Media.Image;

public class UpdateImageValidator : AbstractValidator<UpdateImageCommand>
{
    public UpdateImageValidator(BaseImageValidator baseImageValidator)
    {
        RuleFor(c => c.Image).SetValidator(baseImageValidator);
    }
}