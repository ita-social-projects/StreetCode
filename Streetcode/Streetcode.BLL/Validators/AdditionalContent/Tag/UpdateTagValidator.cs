using FluentValidation;
using Streetcode.BLL.MediatR.AdditionalContent.Tag.Update;

namespace Streetcode.BLL.Validators.AdditionalContent.Tag;

public class UpdateTagValidator : AbstractValidator<UpdateTagCommand>
{
    public UpdateTagValidator(BaseTagValidator baseTagValidator)
    {
        RuleFor(t => t.tag).SetValidator(baseTagValidator);
    }
}