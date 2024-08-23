using FluentValidation;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.SharedResource;
using Streetcode.BLL.Validators.Common;

namespace Streetcode.BLL.Validators.TeamMember.TeamMemberLInk;

public class BaseTeamMemberLinkValidator : AbstractValidator<TeamMemberLinkCreateUpdateDTO>
{
    public const int MaxTeamMemberLinkLength = 255;
    public BaseTeamMemberLinkValidator(IStringLocalizer<CannotConvertNullSharedResource> stringLocalizerCannot)
    {
        RuleFor(l => l.LogoType)
            .NotNull().WithMessage("Logotype is required")
            .IsInEnum().WithMessage(stringLocalizerCannot["CannotCreateTeamMemberLinkWithInvalidLogoType"].Value);
        RuleFor(l => l.TargetUrl)
            .NotNull().WithMessage("Url is required")
            .NotEmpty().WithMessage("Url cannot be empty")
            .MaximumLength(MaxTeamMemberLinkLength).WithMessage("Maximum length of url is 255")
            .MustBeValidUrl();
    }
}