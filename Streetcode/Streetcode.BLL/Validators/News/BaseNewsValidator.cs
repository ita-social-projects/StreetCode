using FluentValidation;
using Streetcode.BLL.DTO.News;

namespace Streetcode.BLL.Validators.News;

public class BaseNewsValidator : AbstractValidator<CreateUpdateNewsDTO>
{
    protected BaseNewsValidator()
    {
        RuleFor(n => n.Title)
            .Length(1, 100)
            .WithMessage("Max Length of Title is 100");
    }
}