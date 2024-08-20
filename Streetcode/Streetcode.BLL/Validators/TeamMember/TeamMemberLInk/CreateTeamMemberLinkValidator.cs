using FluentValidation;
using Streetcode.BLL.MediatR.Team.TeamMembersLinks.Create;

namespace Streetcode.BLL.Validators.TeamMember.TeamMemberLInk;

public class CreateTeamMemberLinkValidator : AbstractValidator<CreateTeamLinkQuery>
{
    public CreateTeamMemberLinkValidator(BaseTeamMemberLinkValidator baseTeamMemberLinkValidator)
    {
        RuleFor(l => l.teamMember).SetValidator(baseTeamMemberLinkValidator);
    }
}