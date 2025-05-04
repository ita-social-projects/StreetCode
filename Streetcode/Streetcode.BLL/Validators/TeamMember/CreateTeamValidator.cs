using FluentValidation;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.MediatR.Team.Create;
using Streetcode.BLL.SharedResource;
using Streetcode.BLL.Validators.TeamMember.TeamMemberLInk;

namespace Streetcode.BLL.Validators.TeamMember;

public class CreateTeamValidator : AbstractValidator<CreateTeamQuery>
{
    public CreateTeamValidator(
        BaseTeamValidator baseTeamValidator,
        BaseTeamMemberLinkValidator baseTeamMemberLinkValidator,
        IStringLocalizer<FailedToValidateSharedResource> localizer,
        IStringLocalizer<FieldNamesSharedResource> fieldLocalizer)
    {
        RuleFor(c => c.teamMember).SetValidator(baseTeamValidator);
        RuleForEach(c => c.teamMember.TeamMemberLinks).SetValidator(baseTeamMemberLinkValidator);

        RuleFor(dto => dto.teamMember.TeamMemberLinks)
            .ValidateTeamMemberLinksCount(localizer, fieldLocalizer);
    }
}