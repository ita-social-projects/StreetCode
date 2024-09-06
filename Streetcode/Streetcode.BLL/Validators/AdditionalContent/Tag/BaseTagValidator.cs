using FluentValidation;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.AdditionalContent.Tag;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.Validators.AdditionalContent.Tag;

public class BaseTagValidator : AbstractValidator<CreateUpdateTagDTO>
{
    private const int TitleMaxLength = 50;
    private readonly IRepositoryWrapper _repositoryWrapper;
    public BaseTagValidator(
        IRepositoryWrapper repositoryWrapper,
        IStringLocalizer<FailedToValidateSharedResource> localizer,
        IStringLocalizer<FieldNamesSharedResource> fieldLocalizer)
    {
        _repositoryWrapper = repositoryWrapper;
        RuleFor(t => t.Title)
            .NotEmpty().WithMessage(x => localizer["CannotBeEmpty", fieldLocalizer["Title"]])
            .MaximumLength(TitleMaxLength)
            .WithMessage(x => localizer["MaxLength", fieldLocalizer["Title"], TitleMaxLength]);
    }

    private async Task<bool> BeUniqueTitle(string title, CancellationToken cancellationToken)
    {
        var existingTagByTitle = await _repositoryWrapper.TagRepository.GetFirstOrDefaultAsync(t => t.Title == title);

        return existingTagByTitle is null;
    }
}