using FluentValidation;
using Streetcode.BLL.MediatR.AdditionalContent.Tag.Create;

namespace Streetcode.BLL.Validators.AdditionalContent.Tag;

public class CreateTagValidator : AbstractValidator<CreateTagQuery>
{
    public CreateTagValidator(BaseTagValidator baseTagValidator)
    {
        RuleFor(t => t.tag).SetValidator(baseTagValidator);
    }
}