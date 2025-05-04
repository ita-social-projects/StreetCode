using FluentValidation;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.MediatR.Team.Update;
using Streetcode.BLL.SharedResource;
using Streetcode.BLL.Validators.TeamMember.TeamMemberLInk;

namespace Streetcode.BLL.Validators.TeamMember;

public class UpdateTeamValidator : AbstractValidator<UpdateTeamQuery>
{
    public const int MaxTeamMemberLinks = 8;

    public UpdateTeamValidator(
        BaseTeamValidator baseTeamValidator,
        BaseTeamMemberLinkValidator baseTeamMemberLinkValidator,
        IStringLocalizer<FailedToValidateSharedResource> localizer,
        IStringLocalizer<FieldNamesSharedResource> fieldLocalizer)
    {
        RuleFor(c => c.TeamMember).SetValidator(baseTeamValidator);
        RuleForEach(c => c.TeamMember.TeamMemberLinks).SetValidator(baseTeamMemberLinkValidator);

        RuleFor(dto => dto.TeamMember.TeamMemberLinks)
            .ValidateTeamMemberLinksCount(localizer, fieldLocalizer);
    }
}