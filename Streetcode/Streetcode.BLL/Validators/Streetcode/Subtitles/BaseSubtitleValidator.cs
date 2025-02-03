using FluentValidation;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.AdditionalContent.Subtitles;
using Streetcode.BLL.SharedResource;

namespace Streetcode.BLL.Validators.Streetcode.Subtitles;

public class BaseSubtitleValidator : AbstractValidator<SubtitleCreateUpdateDto>
{
    public const int SubtitleMaxLength = 255;
    public BaseSubtitleValidator(IStringLocalizer<FailedToValidateSharedResource> localizer, IStringLocalizer<FieldNamesSharedResource> fieldLocalizer)
    {
        RuleFor(dto => dto.SubtitleText)
            .MaximumLength(SubtitleMaxLength).WithMessage(localizer["MaxLength", fieldLocalizer["SubtitleText"], SubtitleMaxLength]);
    }
}