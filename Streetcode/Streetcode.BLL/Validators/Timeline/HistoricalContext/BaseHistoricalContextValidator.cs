using FluentValidation;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Timeline;
using Streetcode.BLL.SharedResource;

namespace Streetcode.BLL.Validators.Timeline.HistoricalContext;

public class BaseHistoricalContextValidator : AbstractValidator<HistoricalContextDTO>
{
    private const int MaxTitleLength = 50;
    public BaseHistoricalContextValidator(IStringLocalizer<FailedToValidateSharedResource> localizer)
    {
        RuleFor(dto => dto.Title)
            .NotEmpty().WithMessage(localizer["TitleRequired"])
            .MaximumLength(MaxTitleLength).WithMessage(localizer["TitleMaxLength", MaxTitleLength]);
    }
}