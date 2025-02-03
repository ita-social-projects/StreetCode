using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Sources;
using Streetcode.BLL.MediatR.Sources.SourceLink.Update;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.Validators.SourceLinkCategory;

public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryCommand>
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    public UpdateCategoryValidator(BaseCategoryValidator baseCategoryValidator, IRepositoryWrapper repositoryWrapper, IStringLocalizer<FailedToValidateSharedResource> localizer, IStringLocalizer<FieldNamesSharedResource> fieldLocalizer)
    {
        _repositoryWrapper = repositoryWrapper;
        RuleFor(c => c.Category).SetValidator(baseCategoryValidator);
        RuleFor(c => c.Category)
            .MustAsync(BeUniqueAsync)
            .WithMessage(localizer["MustBeUnique", fieldLocalizer["Title"]]);
    }

    private async Task<bool> BeUniqueAsync(UpdateSourceLinkCategoryDto category, CancellationToken token)
    {
        var existingCategory = await _repositoryWrapper.SourceCategoryRepository
            .GetFirstOrDefaultAsync(a => a.Title == category.Title);
        if (existingCategory != null)
        {
            return existingCategory.Id == category.Id;
        }

        return true;
    }
}