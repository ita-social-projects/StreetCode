using FluentValidation;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.BLL.SharedResource;

namespace Streetcode.BLL.Validators.Streetcode.Text.Term
{
    public class BaseTermValidator : AbstractValidator<TermCreateDTO>
    {
        public const int TitleMaxLength = 50;
        public const int DescriptionMaxLength = 500;

        public BaseTermValidator(IStringLocalizer<FailedToValidateSharedResource> localizer, IStringLocalizer<FieldNamesSharedResource> fieldLocalizer)
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage(localizer["IsRequired", fieldLocalizer["Title"]])
                .MaximumLength(TitleMaxLength).WithMessage(localizer["MaxLength", fieldLocalizer["Title"], TitleMaxLength]);

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage(localizer["IsRequired", fieldLocalizer["Description"]])
                .MaximumLength(DescriptionMaxLength).WithMessage(localizer["MaxLength", fieldLocalizer["Description"], DescriptionMaxLength]);
        }
    }
}
