using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.News;
using Streetcode.BLL.MediatR.Newss.Create;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.Validators.News;

public class CreateNewsValidator : AbstractValidator<CreateNewsCommand>
{
    private readonly IRepositoryWrapper _repositoryWrapper;

    public CreateNewsValidator(
        BaseNewsValidator baseNewsValidator,
        IStringLocalizer<FailedToValidateSharedResource> localizer,
        IStringLocalizer<FieldNamesSharedResource> fieldLocalizer,
        IRepositoryWrapper repositoryWrapper)
    {
        _repositoryWrapper = repositoryWrapper;

        RuleFor(n => n.newNews).SetValidator(baseNewsValidator);

        RuleFor(n => n.newNews.Title)
            .MustAsync(BeUniqueTitle).WithMessage(x => localizer["MustBeUnique", fieldLocalizer["Title"]]);

        RuleFor(n => n.newNews.Text)
            .MustAsync(BeUniqueText).WithMessage(localizer["MustBeUnique", fieldLocalizer["Text"]]);

        RuleFor(n => n.newNews.URL)
            .MustAsync(BeUniqueUrl!).WithMessage(x => localizer["MustBeUnique", fieldLocalizer["TargetUrl"]]);

        RuleFor(n => n.newNews.ImageId)
            .MustAsync(BeUniqueImageId).WithMessage(x => localizer["MustBeUnique", fieldLocalizer["ImageId"]]);
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

    private async Task<bool> BeUniqueImageId(int imageId, CancellationToken cancellationToken)
    {
        var existingNewsBuImageId = await _repositoryWrapper.NewsRepository.GetSingleOrDefaultAsync(n => n.ImageId == imageId);

        return existingNewsBuImageId is null;
    }
}