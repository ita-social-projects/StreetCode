using FluentValidation;
using FluentValidation.Results;
using Streetcode.BLL.DTO.Sources;
using Streetcode.BLL.MediatR.Sources.SourceLink.Create;

namespace Streetcode.BLL.Validators.SourceLinkCategory;

public class CreateCategoryValidator : AbstractValidator<SourceLinkCreateUpdateCategoryDTO>, IValidator<CreateCategoryCommand>
{
    public CreateCategoryValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title cannot be empty.")
            .MaximumLength(23).WithMessage("Title cannot be longer than 23 characters.")
            .Matches(@"\S").WithMessage("Title cannot be whitespace.");
    }

    public ValidationResult Validate(CreateCategoryCommand instance)
    {
        return Validate(instance.Category);
    }

    public Task<ValidationResult> ValidateAsync(CreateCategoryCommand instance, CancellationToken cancellation = default)
    {
        return ValidateAsync(instance.Category, cancellation);
    }
}