﻿using FluentValidation;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.SharedResource;
using Streetcode.BLL.Validators.Common;
using Streetcode.BLL.Validators.Partners.SourceLinks;

namespace Streetcode.BLL.Validators.Partners;

public class BasePartnersValidator : AbstractValidator<PartnerCreateUpdateDto>
{
    public const int TitleMaxLength = 100;
    public const int DescriptionMaxLength = 450;
    public const int UrlMaxLength = 200;
    public const int UrlTitleMaxLength = 100;
    public BasePartnersValidator(
        PartnerSourceLinkValidator partnerSourceLinkValidator,
        IStringLocalizer<FieldNamesSharedResource> fieldLocalizer,
        IStringLocalizer<FailedToValidateSharedResource> localizer)
    {
        RuleFor(dto => dto.Title)
            .NotEmpty().WithMessage(x => localizer["CannotBeEmpty", fieldLocalizer["Title"]])
            .MaximumLength(TitleMaxLength).WithMessage(x => localizer["MaxLength", fieldLocalizer["Title"], TitleMaxLength]);

        RuleFor(dto => dto.Description)
            .MaximumLength(DescriptionMaxLength).WithMessage(x => localizer["MaxLength", fieldLocalizer["Description"], DescriptionMaxLength]);

        RuleFor(dto => dto.TargetUrl)
            .NotEmpty().When(dto => !string.IsNullOrWhiteSpace(dto.UrlTitle))
            .WithMessage(x => localizer["CannotBeEmptyWithCondition", fieldLocalizer["TargetUrl"], fieldLocalizer["UrlTitle"]]);

        RuleFor(dto => dto.TargetUrl)
            .MaximumLength(UrlMaxLength)
            .WithMessage(x => localizer["MaxLength", fieldLocalizer["TargetUrl"], UrlMaxLength]);

        RuleFor(dto => dto.TargetUrl).MustBeValidUrl()
            .When(dto => dto.TargetUrl != null)
            .WithMessage(x => localizer["ValidUrl", fieldLocalizer["TargetUrl"]]);

        RuleFor(dto => dto.UrlTitle)
            .MaximumLength(UrlTitleMaxLength).WithMessage(x => localizer["MaxLength", fieldLocalizer["UrlTitle"], UrlTitleMaxLength]);

        RuleForEach(dto => dto.PartnerSourceLinks).SetValidator(partnerSourceLinkValidator);
    }
}