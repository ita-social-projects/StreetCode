using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Sources;
using Streetcode.BLL.MediatR.Sources.SourceLink.Create;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.Validators.SourceLinkCategory;

public class CreateCategoryValidator : AbstractValidator<CreateCategoryCommand>
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    public CreateCategoryValidator(BaseCategoryValidator baseCategoryValidator, IRepositoryWrapper repositoryWrapper, IStringLocalizer<FailedToValidateSharedResource> localizer, IStringLocalizer<FieldNamesSharedResource> fieldLocalizer)
    {
        _repositoryWrapper = repositoryWrapper;
        RuleFor(c => c.Category).SetValidator(baseCategoryValidator);
        RuleFor(c => c.Category)
            .MustAsync(BeUniqueAsync)
            .WithMessage(localizer["MustBeUnique", fieldLocalizer["Title"]]);
    }

    private async Task<bool> BeUniqueAsync(SourceLinkCategoryCreateDto category, CancellationToken token)
    {
        var existingCategory = await _repositoryWrapper.SourceCategoryRepository
            .GetFirstOrDefaultAsync(a => a.Title == category.Title);
        return existingCategory == null;
    }
}