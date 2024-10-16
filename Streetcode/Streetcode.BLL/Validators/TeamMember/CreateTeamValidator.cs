using FluentValidation;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.MediatR.Team.Create;
using Streetcode.BLL.Validators.TeamMember.TeamMemberLInk;

namespace Streetcode.BLL.Validators.TeamMember;

public class CreateTeamValidator : AbstractValidator<CreateTeamQuery>
{
    public CreateTeamValidator(BaseTeamValidator baseTeamValidator, BaseTeamMemberLinkValidator baseTeamMemberLinkValidator)
    {
        RuleFor(c => c.teamMember).SetValidator(baseTeamValidator);
        RuleForEach(c => c.teamMember.TeamMemberLinks).SetValidator(baseTeamMemberLinkValidator);
    }
}