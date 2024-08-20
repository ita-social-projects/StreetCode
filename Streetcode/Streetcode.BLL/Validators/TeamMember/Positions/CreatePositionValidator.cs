using FluentValidation;
using Streetcode.BLL.MediatR.Team.Create;

namespace Streetcode.BLL.Validators.TeamMember.Positions;

public class CreatePositionValidator : AbstractValidator<CreatePositionQuery>
{
    public CreatePositionValidator(BasePositionValidator basePositionValidator)
    {
        RuleFor(c => c.position).SetValidator(basePositionValidator);
    }
}