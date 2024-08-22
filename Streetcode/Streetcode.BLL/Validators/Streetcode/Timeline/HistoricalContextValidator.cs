using FluentValidation;
using Streetcode.BLL.DTO.Timeline.Update;

namespace Streetcode.BLL.Validators.Streetcode.TimelineItem;

public class HistoricalContextValidator : AbstractValidator<HistoricalContextCreateUpdateDTO>
{
    public HistoricalContextValidator()
    {
        RuleFor(dto => dto.Title)
            .NotEmpty()
            .MaximumLength(50);
        RuleFor(dto => dto.ModelState)
            .IsInEnum();
    }
}