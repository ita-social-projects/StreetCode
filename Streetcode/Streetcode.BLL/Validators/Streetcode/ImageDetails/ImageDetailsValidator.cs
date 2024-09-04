using FluentValidation;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.Validators.Streetcode.ImageDetails;

public class ImageDetailsValidator : AbstractValidator<ImageDetailsDto>
{
    private const int TitleMaxLength = 100;
    private const int AltMaxLength = 200;
    private IRepositoryWrapper _repositoryWrapper;
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

        RuleFor(dto => dto.ImageId)
            .MustAsync(BeUniqueImageIdInImageDetails);
    }

    private async Task<bool> BeUniqueImageIdInImageDetails(int imadeId, CancellationToken cancellationToken)
    {
        var existingStreetcodeByIndex = await _repositoryWrapper.ImageDetailsRepository.GetFirstOrDefaultAsync(n => n.ImageId == imadeId);

        return existingStreetcodeByIndex is null;
    }
}