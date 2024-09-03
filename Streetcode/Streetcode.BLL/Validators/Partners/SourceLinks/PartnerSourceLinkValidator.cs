using FluentValidation;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Partners.Create;
using Streetcode.BLL.SharedResource;
using Streetcode.BLL.Validators.Common;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.Validators.Partners.SourceLinks;

public class PartnerSourceLinkValidator : AbstractValidator<CreatePartnerSourceLinkDTO>
{
    public const int PartnerLinkMaxLength = 255;
    public PartnerSourceLinkValidator(IStringLocalizer<FieldNamesSharedResource> fieldLocalizer, IStringLocalizer<FailedToValidateSharedResource> localizer)
    {
        RuleFor(dto => dto.LogoType)
            .NotNull().WithMessage(localizer["IsRequired", fieldLocalizer["LogoType"]])
            .IsInEnum().WithMessage(localizer["Invalid", fieldLocalizer["LogoType"]]);

        RuleFor(dto => dto.TargetUrl)
            .NotEmpty().WithMessage(localizer["CannotBeEmpty", fieldLocalizer["SourceLinkUrl"]])
            .MaximumLength(PartnerLinkMaxLength)
            .WithMessage(localizer["MaxLength", fieldLocalizer["SourceLinkUrl"], PartnerLinkMaxLength]);

        RuleFor(dto => dto.TargetUrl)
            .NotEmpty().WithMessage(localizer["CannotBeEmpty", fieldLocalizer["SourceLinkUrl"]])
            .MustBeValidUrl()
            .WithMessage(x => localizer["ValidUrl_UrlDisplayed", fieldLocalizer["SourceLinkUrl"], x.TargetUrl]);

        RuleFor(dto => dto)
            .Must(dto => ValidationExtentions.MatchLogotypeAndUrl(dto.TargetUrl, dto.LogoType))
            .WithMessage(localizer["LogoMustMatchUrl"]);
    }
}