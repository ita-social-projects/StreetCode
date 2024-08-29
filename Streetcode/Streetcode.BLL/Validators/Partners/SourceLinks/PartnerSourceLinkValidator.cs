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
            .WithMessage(localizer["MaxLength", fieldLocalizer["SourceLinkUrl"]]);

        RuleFor(dto => dto.TargetUrl)
            .MustBeValidUrl()
            .WithMessage(x => localizer["ValidUrl_UrlDisplayed", fieldLocalizer["SourceLinkUrl"], x.TargetUrl]);

        RuleFor(dto => dto)
            .Must(MatchLogotypeAndUrl).WithMessage(localizer["LogoMustMatchUrl"]);
    }

    private bool MatchLogotypeAndUrl(CreatePartnerSourceLinkDTO dto)
    {
        bool isUri = Uri.TryCreate(dto.TargetUrl, UriKind.Absolute, out var uri);
        if (!isUri || !Enum.IsDefined(typeof(LogoType), dto.LogoType))
        {
            return false;
        }

        string host = uri!.Host;
        switch (dto.LogoType)
        {
            case LogoType.Behance:
                return host == "www.behance.net" || host == "behance.net";
            case LogoType.Facebook:
                return host == "facebook.com" || host == "www.facebook.com";
            case LogoType.Instagram:
                return host == "instagram.com" || host == "www.instagram.com";
            case LogoType.Linkedin:
                return host == "linkedin.com" || host == "www.linkedin.com";
            case LogoType.Tiktok:
                return host == "tiktok.com" || host == "www.tiktok.com" || host == "vm.tiktok.com";
            case LogoType.Twitter:
                return host == "x.com" || host == "www.x.com";
            case LogoType.YouTube:
                return host == "youtube.com" || host == "www.youtube.com" || host == "youtu.be";
            default:
                throw new ArgumentException("This type of logo is not supported by validation");
        }
    }
}