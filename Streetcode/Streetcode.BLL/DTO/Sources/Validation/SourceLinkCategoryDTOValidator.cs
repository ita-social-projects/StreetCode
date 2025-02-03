using FluentValidation;

namespace Streetcode.BLL.DTO.Sources.Validation
{
    public class SourceLinkCategoryDtoValidator : AbstractValidator<SourceLinkCreateUpdateCategoryDto>
    {
        public SourceLinkCategoryDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title cannot be empty.")
                .MaximumLength(23).WithMessage("Title cannot be longer than 23 characters.")
                .Matches(@"\S").WithMessage("Title cannot be whitespace.");
        }
    }
}
