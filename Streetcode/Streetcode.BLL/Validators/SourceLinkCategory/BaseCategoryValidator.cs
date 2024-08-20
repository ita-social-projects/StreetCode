using FluentValidation;
using Streetcode.BLL.DTO.Sources;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.Validators.SourceLinkCategory;

public class BaseCategoryValidator : AbstractValidator<SourceLinkCreateUpdateCategoryDTO>
{
    public const int MaxCategoryLength = 23;
    private readonly IRepositoryWrapper _repositoryWrapper;
    public BaseCategoryValidator(IRepositoryWrapper repositoryWrapper)
    {
        _repositoryWrapper = repositoryWrapper;
        RuleFor(x => x.Title)
            .NotNull().WithMessage("Title is required")
            .NotEmpty().WithMessage("Title cannot be empty.")
            .MaximumLength(MaxCategoryLength).WithMessage("Title cannot be longer than 23 characters.")
            .Matches(@"\S").WithMessage("Title cannot be whitespace.")
            .MustAsync(BeUniqueAsync).WithMessage("Title must be unique");

        RuleFor(x => x.ImageId)
            .NotNull().WithMessage("ImageId is required")
            .MustAsync(HasExistingImage).WithMessage("Image doesnt exist");
    }

    public async Task<bool> BeUniqueAsync(string categoryTitle, CancellationToken token)
    {
        var existingCategory = await _repositoryWrapper.SourceCategoryRepository
            .GetFirstOrDefaultAsync(a => a.Title == categoryTitle);
        return existingCategory == null;
    }

    public async Task<bool> HasExistingImage(int imageId, CancellationToken token)
    {
        var image = await _repositoryWrapper.ImageRepository
            .GetFirstOrDefaultAsync(a => a.Id == imageId);
        return image != null;
    }
}