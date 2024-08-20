using FluentValidation;
using FluentValidation.Results;
using Streetcode.BLL.MediatR.Sources.SourceLink.Update;

namespace Streetcode.BLL.Validators.SourceLinkCategory;

public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryValidator()
    {
        RuleFor(c => c.Category).SetValidator(new BaseCategoryValidator());
    }
}