using FluentValidation;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Sources;
using Streetcode.BLL.SharedResource;

namespace Streetcode.BLL.Validators.Streetcode.CategoryContent;

public class BaseCategoryContentValidator : AbstractValidator<StreetcodeCategoryContentDto>
{
    public const int TextMaxLength = 6000;
    public BaseCategoryContentValidator(IStringLocalizer<FailedToValidateSharedResource> localizer, IStringLocalizer<FieldNamesSharedResource> fieldLocalizer)
    {
        RuleFor(dto => dto.Text)
            .MaximumLength(TextMaxLength)
            .WithMessage(localizer["MaxLength", fieldLocalizer["CategoryContent"], TextMaxLength]);
    }
}