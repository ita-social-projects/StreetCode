using FluentValidation;
using FluentValidation.Results;
using Streetcode.BLL.DTO.News;
using Streetcode.BLL.MediatR.Newss.Create;

namespace Streetcode.BLL.Validators.News;

public class CreateNewsValidator : AbstractValidator<CreateUpdateNewsDTO>, IValidator<CreateNewsCommand>
{
    public CreateNewsValidator()
    {
        RuleFor(n => n.Title)
            .Length(1, 100)
            .WithMessage("Max Length of Title is 100");
    }

    public ValidationResult Validate(CreateNewsCommand instance)
    {
        return Validate(instance.newNews);
    }

    public Task<ValidationResult> ValidateAsync(CreateNewsCommand instance, CancellationToken cancellation = new CancellationToken())
    {
        return ValidateAsync(instance.newNews, cancellation);
    }
}