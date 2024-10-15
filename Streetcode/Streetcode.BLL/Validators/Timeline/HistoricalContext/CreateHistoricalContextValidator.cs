using FluentValidation;
using Streetcode.BLL.MediatR.Timeline.HistoricalContext.Create;

namespace Streetcode.BLL.Validators.Timeline.HistoricalContext;

public class CreateHistoricalContextValidator : AbstractValidator<CreateHistoricalContextCommand>
{
    public CreateHistoricalContextValidator(BaseHistoricalContextValidator historicalContextValidator)
    {
        RuleFor(c => c.HistoricalContext).SetValidator(historicalContextValidator);
    }
}