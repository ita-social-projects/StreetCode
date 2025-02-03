using FluentValidation;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Sources;
using Streetcode.BLL.SharedResource;
using Streetcode.BLL.Validators.Common;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.Validators.SourceLinkCategory;

public class BaseCategoryValidator : AbstractValidator<SourceLinkCreateUpdateCategoryDto>
{
    public const int MaxCategoryLength = 23;
    private readonly IRepositoryWrapper _repositoryWrapper;
    public BaseCategoryValidator(IRepositoryWrapper repositoryWrapper, IStringLocalizer<FailedToValidateSharedResource> localizer, IStringLocalizer<FieldNamesSharedResource> fieldLocalizer)
    {
        _repositoryWrapper = repositoryWrapper;
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage(localizer["CannotBeEmpty", fieldLocalizer["Title"]])
            .MaximumLength(MaxCategoryLength).WithMessage(localizer["MaxLength", fieldLocalizer["Title"], MaxCategoryLength]);

        RuleFor(x => x.ImageId)
            .MustAsync((imageId, token) => ValidationExtentions.HasExistingImage(_repositoryWrapper, imageId, token)).WithMessage(x => localizer["ImageDoesntExist", x.ImageId]);
    }
}