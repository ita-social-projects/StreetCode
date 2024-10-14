using FluentValidation;

namespace Streetcode.BLL.DTO.Sources.Validation
{
    public class SourceLinkCategoryDTOValidator : AbstractValidator<SourceLinkCreateUpdateCategoryDTO>
    {
        public SourceLinkCategoryDTOValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title cannot be empty.")
                .MaximumLength(23).WithMessage("Title cannot be longer than 23 characters.")
                .Matches(@"\S").WithMessage("Title cannot be whitespace.");
        }
    }
}
