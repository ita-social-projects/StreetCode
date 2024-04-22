using FluentValidation;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Sources;
using Streetcode.BLL.SharedResource;

namespace Streetcode.BLL.DTO.Sources.Validation
{
    public class SourceLinkCategoryDTOValidator : AbstractValidator<SourceLinkCategoryDTO>
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
