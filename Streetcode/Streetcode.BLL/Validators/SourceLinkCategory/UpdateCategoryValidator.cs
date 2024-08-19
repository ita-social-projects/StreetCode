using FluentValidation;
using FluentValidation.Results;
using Streetcode.BLL.MediatR.Sources.SourceLink.Update;

namespace Streetcode.BLL.Validators.SourceLinkCategory;

public class UpdateCategoryValidator : CreateCategoryValidator, IValidator<UpdateCategoryCommand>
{
    public ValidationResult Validate(UpdateCategoryCommand instance)
    {
        return Validate(instance.Category);
    }

    public Task<ValidationResult> ValidateAsync(UpdateCategoryCommand instance, CancellationToken cancellation = new CancellationToken())
    {
        return ValidateAsync(instance.Category, cancellation);
    }
}