using FluentValidation;
using Streetcode.BLL.MediatR.Team.Position.Update;

namespace Streetcode.BLL.Validators.TeamMember.Positions;

public class UpdatePositionValidator : AbstractValidator<UpdateTeamPositionCommand>
{
    public UpdatePositionValidator(BasePositionValidator basePositionValidator)
    {
        RuleFor(c => c.positionDto).SetValidator(basePositionValidator);
    }
}