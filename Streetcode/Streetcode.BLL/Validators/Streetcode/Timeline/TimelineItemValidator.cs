using FluentValidation;
using Streetcode.BLL.DTO.Timeline.Update;
using Streetcode.BLL.Validators.Common;

namespace Streetcode.BLL.Validators.Streetcode.TimelineItem;

public class TimelineItemValidator : AbstractValidator<TimelineItemCreateUpdateDTO>
{
    public TimelineItemValidator(HistoricalContextValidator historicalContextValidator)
    {
        RuleFor(dto => dto.Title)
            .MaximumLength(100)
            .NotEmpty();
        RuleFor(dto => dto.Description)
            .MaximumLength(600)
            .NotEmpty();
        RuleFor(dto => dto.Date)
            .NotEmpty();
        RuleFor(dto => dto.DateViewPattern)
            .IsInEnum();
        RuleFor(dto => dto.ModelState)
            .IsInEnum();
        RuleForEach(dto => dto.HistoricalContexts)
            .SetValidator(historicalContextValidator);
    }
}