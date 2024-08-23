using FluentValidation;
using Streetcode.BLL.DTO.AdditionalContent.Subtitles;

namespace Streetcode.BLL.Validators.Streetcode.Subtitles;

public class BaseSubtitleValidator : AbstractValidator<SubtitleCreateUpdateDTO>
{
    public const int SubtitleMaxLength = 255;
    public BaseSubtitleValidator()
    {
        RuleFor(dto => dto.SubtitleText)
            .MaximumLength(SubtitleMaxLength).WithMessage($"Maximum length of Subtitle is {SubtitleMaxLength}");
    }
}