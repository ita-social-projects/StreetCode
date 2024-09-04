using FluentValidation;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.News;
using Streetcode.BLL.SharedResource;
using Streetcode.BLL.Validators.Common;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.Validators.News;

public class BaseNewsValidator : AbstractValidator<CreateUpdateNewsDTO>
{
    private const int TitleMaxLength = 100;
    private const int UrlMaxLength = 200;
    private const int ImageIdMinValue = 0;
    private readonly IRepositoryWrapper _repositoryWrapper;
    public BaseNewsValidator(IStringLocalizer<FailedToValidateSharedResource> localizer, IStringLocalizer<FieldNamesSharedResource> fieldLocalizer, IRepositoryWrapper repositoryWrapper)
    {
        _repositoryWrapper = repositoryWrapper;

        RuleFor(x => x.Title)
                .NotEmpty().WithMessage(x => localizer["CannotBeEmpty", fieldLocalizer["Title"]])
                .MaximumLength(TitleMaxLength).WithMessage(x => localizer["MaxLength", fieldLocalizer["Title"], TitleMaxLength])
                .MustAsync(BeUniqueTitle).WithMessage(x => localizer["MustBeUnique", fieldLocalizer["Title"]]);

        RuleFor(x => x.Text)
                .NotEmpty().WithMessage(localizer["CannotBeEmpty", fieldLocalizer["Text"]])
                .MustAsync(BeUniqueText).WithMessage(localizer["MustBeUnique", fieldLocalizer["Text"]]);

        RuleFor(x => x.ImageId)
                .GreaterThan(ImageIdMinValue).WithMessage(x => localizer["Invalid", fieldLocalizer["ImageId"]])
                .MustAsync(BeExistingImageId).WithMessage(x => localizer["ImageDoesntExist", x.ImageId]);

        RuleFor(x => x.CreationDate)
                .NotEmpty().WithMessage(x => localizer["IsRequired", fieldLocalizer["CreationDate"]]);

        RuleFor(x => x.URL)
                .NotEmpty().WithMessage(x => localizer["CannotBeEmpty", fieldLocalizer["TargetUrl"]])
                .MaximumLength(UrlMaxLength).WithMessage(localizer["MaxLength", fieldLocalizer["TargetUrl"], UrlMaxLength])
                .Matches("^[a-zA-Z0-9-]*$").WithMessage(x => localizer["InvalidNewsUrl"])
                .MustAsync(BeUniqueUrl!).WithMessage(x => localizer["MustBeUnique", fieldLocalizer["TargetUrl"]]);
    }

    private async Task<bool> BeUniqueTitle(string title, CancellationToken cancellationToken)
    {
        var existingNewsByTitle = await _repositoryWrapper.NewsRepository.GetFirstOrDefaultAsync(n => n.Title == title);

        return existingNewsByTitle is null;
    }

    private async Task<bool> BeUniqueText(string text, CancellationToken cancellationToken)
    {
        var existingNewsByText = await _repositoryWrapper.NewsRepository.GetSingleOrDefaultAsync(n => n.Text == text);

        return existingNewsByText is null;
    }

    private async Task<bool> BeUniqueUrl(string url, CancellationToken cancellationToken)
    {
        var existingNewsByUrl = await _repositoryWrapper.NewsRepository.GetSingleOrDefaultAsync(n => n.URL == url);

        return existingNewsByUrl is null;
    }

    private async Task<bool> BeExistingImageId(int imageId, CancellationToken cancellationToken)
    {
        var existingImage = await _repositoryWrapper.ImageRepository.GetFirstOrDefaultAsync(i => i.Id == imageId);

        return existingImage is not null;
    }
}