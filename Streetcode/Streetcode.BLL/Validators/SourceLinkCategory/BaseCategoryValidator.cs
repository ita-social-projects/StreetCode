using FluentValidation;
using Streetcode.BLL.DTO.Sources;

namespace Streetcode.BLL.Validators.SourceLinkCategory;

public class BaseCategoryValidator : AbstractValidator<SourceLinkCreateUpdateCategoryDTO>
{
    public BaseCategoryValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title cannot be empty.")
            .MaximumLength(23).WithMessage("Title cannot be longer than 23 characters.")
            .Matches(@"\S").WithMessage("Title cannot be whitespace.");
    }
}