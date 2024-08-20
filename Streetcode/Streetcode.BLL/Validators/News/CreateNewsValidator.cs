using FluentValidation;
using FluentValidation.Results;
using Streetcode.BLL.DTO.News;
using Streetcode.BLL.MediatR.Newss.Create;

namespace Streetcode.BLL.Validators.News;

public class CreateNewsValidator : AbstractValidator<CreateNewsCommand>
{
    public CreateNewsValidator(BaseNewsValidator baseNewsValidator)
    {
        RuleFor(n => n.newNews).SetValidator(baseNewsValidator);
    }
}