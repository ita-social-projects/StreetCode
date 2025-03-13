using FluentValidation;
using Streetcode.BLL.MediatR.Event.Create;

namespace Streetcode.BLL.Validators.Event;

public class CreateEventValidator : AbstractValidator<CreateEventCommand>
{
    public CreateEventValidator(BaseEventValidator baseEventValidator)
    {
        RuleFor(e => e.Event).SetValidator(baseEventValidator);
    }
}