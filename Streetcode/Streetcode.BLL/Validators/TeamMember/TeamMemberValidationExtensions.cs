using FluentValidation;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.SharedResource;

namespace Streetcode.BLL.Validators.TeamMember;

public static class TeamMemberValidationExtensions
{
    public const int MaxTeamMemberLinks = 8;

    public static IRuleBuilderOptions<T, IList<TItem>> ValidateTeamMemberLinksCount<T, TItem>(
        this IRuleBuilder<T, IList<TItem>> ruleBuilder,
        IStringLocalizer<FailedToValidateSharedResource> localizer,
        IStringLocalizer<FieldNamesSharedResource> fieldLocalizer)
    {
        return ruleBuilder
            .Must(links => links == null || links.Count <= MaxTeamMemberLinks)
            .WithMessage(localizer["MaxCountExceeded", fieldLocalizer["TeamMemberLinks"], MaxTeamMemberLinks])
            .WithName(fieldLocalizer["TeamMemberLinks"]);
    }
}