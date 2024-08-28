using FluentValidation;
using Streetcode.BLL.DTO.Partners.Create;
using Streetcode.BLL.Validators.Common;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.Validators.Partners.SourceLinks;

public class PartnerSourceLinkValidator : AbstractValidator<CreatePartnerSourceLinkDTO>
{
    public const int PartnerLinkMaxLength = 255;
    public PartnerSourceLinkValidator()
    {
        RuleFor(dto => dto.LogoType)
            .NotNull().WithMessage("{PropertyName} is required")
            .IsInEnum().WithMessage("Incorrect logotype");

        RuleFor(dto => dto.TargetUrl)
            .NotEmpty().WithMessage("{PropertyName} cannot be empty")
            .MaximumLength(PartnerLinkMaxLength)
            .WithMessage($"Maximum length of {{PropertyName}} is {PartnerLinkMaxLength}");

        RuleFor(dto => dto.TargetUrl)
            .MustBeValidUrl()
            .WithName(dto => $"Source url '{dto.TargetUrl}'")
            .WithMessage("{PropertyName} must be valid url");

        RuleFor(dto => dto)
            .Must(MatchLogotypeAndUrl).WithMessage("Logo must match corresponding url");
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