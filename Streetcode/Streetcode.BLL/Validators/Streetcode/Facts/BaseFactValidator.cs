using FluentValidation;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Streetcode.TextContent.Fact;
using Streetcode.BLL.SharedResource;

namespace Streetcode.BLL.Validators.Streetcode.Facts;

public class BaseFactValidator : AbstractValidator<FactUpdateCreateDto>
{
    public const int TitleMaxLength = 68;
    public const int ContentMaxLength = 600;
    public const int ImageDescriptionMaxLength = 200;
    public BaseFactValidator(IStringLocalizer<FailedToValidateSharedResource> localizer, IStringLocalizer<FieldNamesSharedResource> fieldLocalizer)
    {
        RuleFor(dto => dto.Title)
            .NotEmpty().WithMessage(localizer["CannotBeEmpty", fieldLocalizer["FactTitle"]])
            .MaximumLength(TitleMaxLength).WithMessage(localizer["MaxLength", fieldLocalizer["FactTitle"], TitleMaxLength]);

        RuleFor(dto => dto.FactContent)
            .NotEmpty().WithMessage(localizer["CannotBeEmpty", fieldLocalizer["FactContent"]])
            .MaximumLength(ContentMaxLength).WithMessage(localizer["MaxLength", fieldLocalizer["FactContent"], ContentMaxLength]);

        RuleFor(dto => dto.ImageDescription)
            .MaximumLength(ImageDescriptionMaxLength)
            .WithMessage(localizer["MaxLength", fieldLocalizer["FactImageDescription"], ContentMaxLength]);
    }
}