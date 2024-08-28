using FluentValidation;
using Streetcode.BLL.DTO.Sources;

namespace Streetcode.BLL.Validators.Streetcode.CategoryContent;

public class BaseCategoryContentValidator : AbstractValidator<StreetcodeCategoryContentDTO>
{
    public const int TextMaxLength = 6000;
    public BaseCategoryContentValidator()
    {
        RuleFor(dto => dto.Text)
            .MaximumLength(TextMaxLength)
            .WithName("Category content text")
            .WithMessage($"Maximum length of {{PropertyName}} is {TextMaxLength}");
    }
}