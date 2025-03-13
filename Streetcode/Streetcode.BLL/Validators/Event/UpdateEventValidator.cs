using FluentValidation;
using Streetcode.BLL.MediatR.Event.Update;

namespace Streetcode.BLL.Validators.Event;
public class UpdateEventValidator : AbstractValidator<UpdateEventCommand>
{
    public UpdateEventValidator(BaseEventValidator baseEventValidator)
    {
        RuleFor(e => e.Event).SetValidator(baseEventValidator);
    }
}