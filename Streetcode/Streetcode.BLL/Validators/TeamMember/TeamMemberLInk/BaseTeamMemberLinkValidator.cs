using FluentValidation;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.SharedResource;

namespace Streetcode.BLL.Validators.TeamMember.TeamMemberLInk;

public class BaseTeamMemberLinkValidator : AbstractValidator<TeamMemberLinkCreateUpdateDTO>
{
    public BaseTeamMemberLinkValidator(IStringLocalizer<CannotConvertNullSharedResource> stringLocalizerCannot)
    {
        RuleFor(l => l.LogoType)
            .IsInEnum().WithMessage(stringLocalizerCannot["CannotCreateTeamMemberLinkWithInvalidLogoType"].Value);
        RuleFor(l => l.TargetUrl)
            .NotEmpty().WithMessage("Url cannot be empty")
            .MaximumLength(255).WithMessage("Maximum length of url is 255")
            .Must(BeUrl).WithMessage("Url is incorrect");
    }

    private bool BeUrl(string value)
    {
        return Uri.TryCreate(value, UriKind.Absolute, out _);
    }
}