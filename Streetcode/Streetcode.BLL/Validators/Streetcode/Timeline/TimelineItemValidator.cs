using FluentValidation;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Timeline.Update;
using Streetcode.BLL.SharedResource;

namespace Streetcode.BLL.Validators.Streetcode.TimelineItem;

public class TimelineItemValidator : AbstractValidator<TimelineItemCreateUpdateDTO>
{
    public const int TitleMaxLength = 100;
    public const int DescriptionMaxLength = 600;
    public TimelineItemValidator(
        HistoricalContextValidator historicalContextValidator,
        IStringLocalizer<FailedToValidateSharedResource> localizer,
        IStringLocalizer<FieldNamesSharedResource> fieldLocalizer)
    {
        RuleFor(dto => dto.Title)
            .NotEmpty().WithMessage(localizer["CannotBeEmpty", fieldLocalizer["TimelineItemTitle"]])
            .MaximumLength(TitleMaxLength).WithMessage(localizer["MaxLength", fieldLocalizer["TimelineItemTitle"], TitleMaxLength]);

        RuleFor(dto => dto.Description)
            .NotEmpty().WithMessage(localizer["CannotBeEmpty", fieldLocalizer["TimelineItemDescription"]])
            .MaximumLength(DescriptionMaxLength).WithMessage(localizer["MaxLength", fieldLocalizer["TimelineItemDescription"], DescriptionMaxLength]);

        RuleFor(dto => dto.Date)
            .NotEmpty().WithMessage(localizer["CannotBeEmpty", fieldLocalizer["TimelineItemDate"]]);

        RuleFor(dto => dto.DateViewPattern)
            .IsInEnum().WithMessage(localizer["Invalid", fieldLocalizer["DateFormat"]]);

        RuleFor(dto => dto.ModelState)
            .IsInEnum().WithMessage(localizer["Invalid", fieldLocalizer["ModelState"]]);

        RuleForEach(dto => dto.HistoricalContexts)
            .SetValidator(historicalContextValidator);
    }
}