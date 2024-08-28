using FluentValidation;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.Validators.Common;
using Streetcode.BLL.Validators.Partners.SourceLinks;

namespace Streetcode.BLL.Validators.Partners;

public class BasePartnersValidator : AbstractValidator<PartnerCreateUpdateDto>
{
    public const int TitleMaxLength = 100;
    public const int DescriptionMaxLength = 450;
    public const int UrlMaxLength = 200;
    public BasePartnersValidator(PartnerSourceLinkValidator partnerSourceLinkValidator)
    {
        RuleFor(dto => dto.Title)
            .NotEmpty().WithMessage("{PropertyName} cannot be empty")
            .MaximumLength(TitleMaxLength).WithMessage($"Maximum length of {{PropertyName}} is {TitleMaxLength}");

        RuleFor(dto => dto.Description)
            .MaximumLength(DescriptionMaxLength).WithMessage($"Maximum length of {{PropertyName}} is {DescriptionMaxLength}");

        RuleFor(dto => dto.TargetUrl)
            .NotEmpty().When(dto => !string.IsNullOrWhiteSpace(dto.UrlTitle))
            .WithMessage("The partner website url is missing")
            .MaximumLength(UrlMaxLength).WithMessage($"Maximum length of {{PropertyName}} is {UrlMaxLength}");

        RuleFor(dto => dto.TargetUrl).MustBeValidUrl()
            .When(dto => dto.TargetUrl != null)
            .WithMessage("{PropertyName} must be valid url");

        RuleFor(dto => dto.UrlTitle)
            .MaximumLength(UrlMaxLength).WithMessage($"Maximum length of {{PropertyName}} is {UrlMaxLength}");

        RuleForEach(dto => dto.PartnerSourceLinks).SetValidator(partnerSourceLinkValidator);
    }
}