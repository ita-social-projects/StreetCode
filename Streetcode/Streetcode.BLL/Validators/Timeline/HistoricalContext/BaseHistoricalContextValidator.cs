using FluentValidation;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Timeline;
using Streetcode.BLL.SharedResource;

namespace Streetcode.BLL.Validators.Timeline.HistoricalContext;

public class BaseHistoricalContextValidator : AbstractValidator<HistoricalContextDto>
{
    public const int MaxTitleLength = 50;
    public BaseHistoricalContextValidator(IStringLocalizer<FailedToValidateSharedResource> localizer, IStringLocalizer<FieldNamesSharedResource> fieldLocalizer)
    {
        RuleFor(dto => dto.Title)
            .NotEmpty().WithMessage(localizer["IsRequired", fieldLocalizer["Title"]])
            .MaximumLength(MaxTitleLength).WithMessage(localizer["MaxLength", fieldLocalizer["Title"], MaxTitleLength]);
    }
}