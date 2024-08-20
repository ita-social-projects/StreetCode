using FluentValidation;
using FluentValidation.Results;
using Streetcode.BLL.MediatR.Newss.Update;

namespace Streetcode.BLL.Validators.News;

public class UpdateNewsValidator : CreateNewsValidator, IValidator<UpdateNewsCommand>
{
    public ValidationResult Validate(UpdateNewsCommand instance)
    {
        return Validate(instance.news);
    }

    public Task<ValidationResult> ValidateAsync(UpdateNewsCommand instance, CancellationToken cancellation = new CancellationToken())
    {
        return ValidateAsync(instance.news, cancellation);
    }
}
