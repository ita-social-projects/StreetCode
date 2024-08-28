using FluentValidation;
using Streetcode.BLL.MediatR.Timeline.HistoricalContext.Update;

namespace Streetcode.BLL.Validators.Timeline.HistoricalContext;

public class UpdateHistoricalContextValidator : AbstractValidator<UpdateHistoricalContextCommand>
{
    public UpdateHistoricalContextValidator(BaseHistoricalContextValidator historicalContextValidator)
    {
        RuleFor(c => c.HistoricalContext)
            .SetValidator(historicalContextValidator);
    }
}