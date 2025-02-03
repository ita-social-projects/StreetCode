using System.Diagnostics;
using FluentValidation;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.DTO.Team.Abstractions;
using Streetcode.BLL.SharedResource;
using Streetcode.BLL.Validators.Common;

namespace Streetcode.BLL.Validators.TeamMember.TeamMemberLInk;

public class BaseTeamMemberLinkValidator : AbstractValidator<TeamMemberLinkCreateUpdateDto>
{
    public const int MaxTeamMemberLinkLength = 255;
    public BaseTeamMemberLinkValidator(IStringLocalizer<FailedToValidateSharedResource> localizer, IStringLocalizer<FieldNamesSharedResource> fieldLocalizer)
    {
        RuleFor(l => l.LogoType)
            .NotNull().WithMessage(localizer["IsRequired", fieldLocalizer["LogoType"]])
            .IsInEnum().WithMessage(localizer["Invalid", fieldLocalizer["LogoType"]]);

        RuleFor(l => l.TargetUrl)
            .NotEmpty().WithMessage(x => localizer["CannotBeEmpty", fieldLocalizer["SourceLinkUrl"]])
            .MaximumLength(MaxTeamMemberLinkLength).WithMessage(localizer["MaxLength", fieldLocalizer["SourceLinkUrl"], MaxTeamMemberLinkLength])
            .MustBeValidUrl().WithMessage(x => localizer["ValidUrl_UrlDisplayed", fieldLocalizer["SourceLinkUrl"], x.TargetUrl]);

        RuleFor(dto => dto)
            .Must(dto => ValidationExtentions.MatchLogotypeAndUrl(dto.TargetUrl, dto.LogoType))
            .WithMessage(localizer["LogoMustMatchUrl"]);
    }
}