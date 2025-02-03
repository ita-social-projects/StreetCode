using FluentValidation;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Timeline.Update;
using Streetcode.BLL.SharedResource;

namespace Streetcode.BLL.Validators.Streetcode.TimelineItem;

public class HistoricalContextValidator : AbstractValidator<HistoricalContextCreateUpdateDto>
{
    public const int TitleMaxLength = 50;
    public HistoricalContextValidator(IStringLocalizer<FailedToValidateSharedResource> localizer, IStringLocalizer<FieldNamesSharedResource> fieldLocalizer)
    {
        RuleFor(dto => dto.Title)
            .NotEmpty().WithMessage(localizer["CannotBeEmpty", fieldLocalizer["HistoricalContextTitle"]])
            .MaximumLength(TitleMaxLength).WithMessage(localizer["MaxLength", fieldLocalizer["HistoricalContextTitle"], TitleMaxLength]);
        RuleFor(dto => dto.ModelState)
            .IsInEnum().WithMessage(localizer["Invalid", fieldLocalizer["ModelState"]]);
    }
}