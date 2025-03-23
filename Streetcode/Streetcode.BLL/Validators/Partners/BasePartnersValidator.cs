using FluentValidation;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.SharedResource;
using Streetcode.BLL.Validators.Common;
using Streetcode.BLL.Validators.Partners.SourceLinks;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.Validators.Partners;

public class BasePartnersValidator : AbstractValidator<PartnerCreateUpdateDto>
{
    public const int TitleMaxLength = 100;
    public const int DescriptionMaxLength = 450;
    public const int UrlMaxLength = 200;
    public const int UrlTitleMaxLength = 100;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public BasePartnersValidator(
        PartnerSourceLinkValidator partnerSourceLinkValidator,
        IStringLocalizer<FieldNamesSharedResource> fieldLocalizer,
        IStringLocalizer<FailedToValidateSharedResource> localizer,
        IStringLocalizer<NoSharedResource> stringLocalizerNo,
        IRepositoryWrapper repositoryWrapper)
    {
        _repositoryWrapper = repositoryWrapper;
        RuleFor(dto => dto.Title)
            .NotEmpty().WithMessage(x => localizer["CannotBeEmpty", fieldLocalizer["Title"]])
            .MaximumLength(TitleMaxLength).WithMessage(x => localizer[ValidationMessageConstants.MaxLength, fieldLocalizer["Title"], TitleMaxLength]);

        RuleFor(dto => dto.Description)
            .MaximumLength(DescriptionMaxLength).WithMessage(x => localizer[ValidationMessageConstants.MaxLength, fieldLocalizer["Description"], DescriptionMaxLength]);

        RuleFor(dto => dto.TargetUrl)
            .NotEmpty().When(dto => !string.IsNullOrWhiteSpace(dto.UrlTitle))
            .WithMessage(x => localizer["CannotBeEmptyWithCondition", fieldLocalizer["TargetUrl"], fieldLocalizer["UrlTitle"]]);

        RuleFor(dto => dto.TargetUrl)
            .MaximumLength(UrlMaxLength)
            .WithMessage(x => localizer[ValidationMessageConstants.MaxLength, fieldLocalizer["TargetUrl"], UrlMaxLength]);

        RuleFor(dto => dto.TargetUrl).MustBeValidUrl()
            .When(dto => dto.TargetUrl != null)
            .WithMessage(x => localizer["ValidUrl", fieldLocalizer["TargetUrl"]]);

        RuleFor(dto => dto.UrlTitle)
            .MaximumLength(UrlTitleMaxLength).WithMessage(x => localizer[ValidationMessageConstants.MaxLength, fieldLocalizer["UrlTitle"], UrlTitleMaxLength]);

        RuleForEach(dto => dto.PartnerSourceLinks)
            .SetValidator(partnerSourceLinkValidator);

        RuleFor(dto => dto.LogoId)
            .MustAsync((imageId, token) => ValidationExtentions.HasExistingImage(_repositoryWrapper, imageId, token))
            .WithMessage(x => localizer["ImageDoesntExist", x.LogoId].Value);

        RuleFor(dto => dto.Streetcodes.Select(id => id.Id).ToList())
            .MustAsync((_, listIds, context, _) => BeValidStreetcodeIds(context, listIds))
            .WithMessage(x => stringLocalizerNo["NoExistingStreetcodeWithId", "{ListIds}"].Value);
    }

    private async Task<bool> BeValidStreetcodeIds(ValidationContext<PartnerCreateUpdateDto> context, List<int> streetcodeIds)
    {
        var existingStreetcodeIds = (await _repositoryWrapper.StreetcodeRepository
                .GetAllAsync(s => streetcodeIds.Contains(s.Id)))
            .Select(s => s.Id)
            .ToList();

        var missingIds = streetcodeIds.Except(existingStreetcodeIds).ToList();
        if (missingIds.Any())
        {
            context.MessageFormatter.AppendArgument("ListIds",  string.Join(", ", missingIds));
            return false;
        }

        return true;
    }
}