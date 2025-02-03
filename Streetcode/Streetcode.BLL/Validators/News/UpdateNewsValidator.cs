using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.News;
using Streetcode.BLL.MediatR.Newss.Update;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.Validators.News;

public class UpdateNewsValidator : AbstractValidator<UpdateNewsCommand>
{
    private readonly IRepositoryWrapper _repositoryWrapper;

    public UpdateNewsValidator(
        IStringLocalizer<FailedToValidateSharedResource> localizer,
        IStringLocalizer<FieldNamesSharedResource> fieldLocalizer,
        IRepositoryWrapper repositoryWrapper,
        BaseNewsValidator baseNewsValidator)
    {
        _repositoryWrapper = repositoryWrapper;
        RuleFor(n => n.news).SetValidator(baseNewsValidator);
        RuleFor(c => c.news)
            .MustAsync(BeUniqueTitle).WithMessage(x => localizer["MustBeUnique", fieldLocalizer["Title"]])
            .MustAsync(BeUniqueText).WithMessage(localizer["MustBeUnique", fieldLocalizer["Text"]])
            .MustAsync(BeUniqueUrl!).WithMessage(x => localizer["MustBeUnique", fieldLocalizer["TargetUrl"]])
            .MustAsync(BeUniqueImageId).WithMessage(x => localizer["MustBeUnique", fieldLocalizer["ImageId"]]);
    }

    private async Task<bool> BeUniqueTitle(UpdateNewsDto dto, CancellationToken cancellationToken)
    {
        var existingNewsByTitle = await _repositoryWrapper.NewsRepository.GetFirstOrDefaultAsync(n => n.Title == dto.Title);

        if (existingNewsByTitle is not null)
        {
            return existingNewsByTitle.Id == dto.Id;
        }

        return true;
    }

    private async Task<bool> BeUniqueText(UpdateNewsDto dto, CancellationToken cancellationToken)
    {
        var existingNewsByText = await _repositoryWrapper.NewsRepository.GetSingleOrDefaultAsync(n => n.Text == dto.Text);

        if (existingNewsByText is not null)
        {
            return existingNewsByText.Id == dto.Id;
        }

        return true;
    }

    private async Task<bool> BeUniqueUrl(UpdateNewsDto dto, CancellationToken cancellationToken)
    {
        var existingNewsByUrl = await _repositoryWrapper.NewsRepository.GetSingleOrDefaultAsync(n => n.URL == dto.URL);

        if (existingNewsByUrl is not null)
        {
            return existingNewsByUrl.Id == dto.Id;
        }

        return existingNewsByUrl is null;
    }

    private async Task<bool> BeUniqueImageId(UpdateNewsDto dto, CancellationToken cancellationToken)
    {
        var existingNewsBuImageId = await _repositoryWrapper.NewsRepository.GetSingleOrDefaultAsync(n => n.ImageId == dto.ImageId);

        if (existingNewsBuImageId is not null)
        {
            return existingNewsBuImageId.Id == dto.Id;
        }

        return existingNewsBuImageId is null;
    }
}
