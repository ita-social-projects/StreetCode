using FluentValidation;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.SharedResource;
using Streetcode.BLL.Validators.Common;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.Validators.Streetcode.ImageDetails;

public class ImageDetailsValidator : AbstractValidator<ImageDetailsDto>
{
    public const int TitleMaxLength = 100;
    public const int AltMaxLength = 200;
    private readonly IRepositoryWrapper _repositoryWrapper;
    public ImageDetailsValidator(
        IStringLocalizer<FailedToValidateSharedResource> localizer,
        IStringLocalizer<FieldNamesSharedResource> fieldLocalizer,
        IRepositoryWrapper repositoryWrapper)
    {
        _repositoryWrapper = repositoryWrapper;

        RuleFor(dto => dto.Title)
            .MaximumLength(TitleMaxLength).WithMessage(localizer["MaxLength", fieldLocalizer["ImageTitle"], TitleMaxLength]);

        RuleFor(dto => dto.Alt)
            .MaximumLength(AltMaxLength).WithMessage(localizer["MaxLength", fieldLocalizer["Alt"], AltMaxLength]);

        RuleFor(dto => dto)
            .MustAsync(BeUniqueImageIdInImageDetails).WithMessage(x => localizer["MustBeUnique", fieldLocalizer["ImageId"]]);
        
        RuleFor(dto => dto.ImageId)
            .MustAsync((imageId, token) => ValidationExtentions.HasExistingImage(_repositoryWrapper, imageId, token)).WithMessage(x => localizer["ImageDoesntExist", x.ImageId]);
    }

    private async Task<bool> BeUniqueImageIdInImageDetails(ImageDetailsDto imageDetails, CancellationToken cancellationToken)
    {
        var existingImageDetails = await _repositoryWrapper.ImageDetailsRepository.GetFirstOrDefaultAsync(n => n.ImageId == imageDetails.ImageId);
        if (existingImageDetails != null)
        {
            return existingImageDetails.Id == imageDetails.Id;
        }

        return true;
    }
}