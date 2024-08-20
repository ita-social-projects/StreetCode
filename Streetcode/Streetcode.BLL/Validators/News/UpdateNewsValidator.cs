using FluentValidation;
using FluentValidation.Results;
using Streetcode.BLL.MediatR.Newss.Update;

namespace Streetcode.BLL.Validators.News;

public class UpdateNewsValidator : AbstractValidator<UpdateNewsCommand>
{
    public UpdateNewsValidator(BaseNewsValidator baseNewsValidator)
    {
        RuleFor(n => n.news).SetValidator(baseNewsValidator);
    }
}
