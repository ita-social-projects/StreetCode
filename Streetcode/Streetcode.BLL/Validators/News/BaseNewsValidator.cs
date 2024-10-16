using FluentValidation;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.News;
using Streetcode.BLL.SharedResource;
using Streetcode.BLL.Validators.Common;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.Validators.News;

public class BaseNewsValidator : AbstractValidator<CreateUpdateNewsDTO>
{
    public const int TitleMaxLength = 100;
    public const int UrlMaxLength = 200;
    private const int ImageIdMinValue = 0;
    private readonly IRepositoryWrapper _repositoryWrapper;
    public BaseNewsValidator(
        IStringLocalizer<FailedToValidateSharedResource> localizer,
        IStringLocalizer<FieldNamesSharedResource> fieldLocalizer,
        IRepositoryWrapper repositoryWrapper)
    {
        _repositoryWrapper = repositoryWrapper;

        RuleFor(x => x.Title)
                .NotEmpty().WithMessage(x => localizer["CannotBeEmpty", fieldLocalizer["Title"]])
                .MaximumLength(TitleMaxLength).WithMessage(x => localizer["MaxLength", fieldLocalizer["Title"], TitleMaxLength]);

        RuleFor(x => x.Text)
            .NotEmpty().WithMessage(localizer["CannotBeEmpty", fieldLocalizer["Text"]]);

        RuleFor(x => x.ImageId)
                .GreaterThan(ImageIdMinValue).WithMessage(x => localizer["Invalid", fieldLocalizer["ImageId"]])
                .MustAsync(BeExistingImageId).WithMessage(x => localizer["ImageDoesntExist", x.ImageId]);

        RuleFor(x => x.CreationDate)
                .NotEmpty().WithMessage(x => localizer["IsRequired", fieldLocalizer["CreationDate"]]);

        RuleFor(x => x.URL)
            .NotEmpty().WithMessage(x => localizer["CannotBeEmpty", fieldLocalizer["TargetUrl"]])
            .MaximumLength(UrlMaxLength).WithMessage(localizer["MaxLength", fieldLocalizer["TargetUrl"], UrlMaxLength])
            .Matches(@"^[a-z0-9-]*$").WithMessage(x => localizer["InvalidNewsUrl"]);
    }

    private async Task<bool> BeExistingImageId(int imageId, CancellationToken cancellationToken)
    {
        var existingImage = await _repositoryWrapper.ImageRepository.GetFirstOrDefaultAsync(i => i.Id == imageId);

        return existingImage is not null;
    }
}