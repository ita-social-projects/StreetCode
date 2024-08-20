using FluentValidation;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.MediatR.Team.Update;
using Streetcode.BLL.Validators.TeamMember.TeamMemberLInk;

namespace Streetcode.BLL.Validators.TeamMember;

public class UpdateTeamValidator : AbstractValidator<UpdateTeamQuery>
{
    public UpdateTeamValidator(BaseTeamValidator<TeamMemberLinkDTO> baseTeamValidator)
    {
        RuleFor(c => c.TeamMember).SetValidator(baseTeamValidator);
    }
}