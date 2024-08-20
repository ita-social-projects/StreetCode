using FluentValidation;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.News;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.Validators.News;

public class BaseNewsValidator : AbstractValidator<CreateUpdateNewsDTO>
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    public BaseNewsValidator(IStringLocalizer<FailedToValidateSharedResource> localizer, IRepositoryWrapper repositoryWrapper)
    {
        _repositoryWrapper = repositoryWrapper;

        RuleFor(x => x.Title)
                .NotEmpty().WithMessage(x => localizer["TitleRequired"])
                .MaximumLength(100).WithMessage(x => localizer["TitleMaxLength"])
                .MustAsync(BeUniqueTitle).WithMessage(x => localizer["TitleNewsAlreadyExists"]);

        RuleFor(x => x.Text)
                .NotEmpty().WithMessage(x => localizer["TextRequired"])
                .MustAsync(BeUniqueText).WithMessage(x => localizer["TextNewsAlreadyExists"]);

        RuleFor(x => x.ImageId)
                .GreaterThan(0).WithMessage(x => localizer["InvalidImageIdValue"])
                .MustAsync(BeExistingImageId).WithMessage(x => localizer["ImageIdNotExists"]);

        RuleFor(x => x.CreationDate)
                .NotEmpty().WithMessage(x => localizer["CreationDateRequired"]);

        RuleFor(x => x.URL)
                .NotEmpty().WithMessage(x => localizer["URLRequired"])
                .Matches("^[a-zA-Z0-9-]*$").WithMessage(x => localizer["URLInvalid"])
                .MustAsync(BeUniqueUrl).WithMessage(x => localizer["URLAlreadyExistsNews"]);
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