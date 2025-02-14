using FluentValidation;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Streetcode.Update;
using Streetcode.BLL.Enums;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.Update;
using Streetcode.BLL.SharedResource;
using Streetcode.BLL.Validators.AdditionalContent.Tag;
using Streetcode.BLL.Validators.Common;
using Streetcode.BLL.Validators.Streetcode.CategoryContent;
using Streetcode.BLL.Validators.Streetcode.Facts;
using Streetcode.BLL.Validators.Streetcode.Subtitles;
using Streetcode.BLL.Validators.Streetcode.Text;
using Streetcode.BLL.Validators.Streetcode.Video;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.Validators.Streetcode;

public class UpdateStreetcodeValidator : AbstractValidator<UpdateStreetcodeCommand>
{
    private readonly IRepositoryWrapper _repositoryWrapper;

    public UpdateStreetcodeValidator(
        IRepositoryWrapper repositoryWrapper,
        BaseStreetcodeValidator baseStreetcodeValidator,
        BaseTextValidator baseTextValidator,
        BaseSubtitleValidator baseSubtitleValidator,
        BaseTagValidator tagValidator,
        BaseFactValidator baseFactValidator,
        BaseVideoValidator videoValidator,
        BaseCategoryContentValidator categoryContentValidator,
        IStringLocalizer<FailedToValidateSharedResource> localizer,
        IStringLocalizer<FieldNamesSharedResource> fieldLocalizer)
    {
        _repositoryWrapper = repositoryWrapper;
        RuleFor(c => c.Streetcode).SetValidator(baseStreetcodeValidator);
        RuleFor(c => c.Streetcode)
            .MustAsync(BeUniqueIndex).WithMessage(x => localizer["MustBeUnique", fieldLocalizer["Index"]]);
        RuleFor(c => c.Streetcode!.ArBlockUrl)
            .MustBeValidUrl()
            .When(c => !string.IsNullOrEmpty(c.Streetcode.ArBlockUrl))
            .WithMessage(localizer["ValidUrl", fieldLocalizer["ARBlockURL"]]);

        RuleFor(c => c.Streetcode.Text!.Title)
            .NotEmpty()
            .When(c => c.Streetcode.Videos is not null && c.Streetcode.Text is not null && c.Streetcode.Videos.Any())
            .WithMessage(localizer["CannotBeEmptyWithCondition", fieldLocalizer["Title"], fieldLocalizer["Video"]]);
        RuleForEach(c => c.Streetcode.Videos)
            .SetValidator(videoValidator);

        RuleFor(c => c.Streetcode.Text).SetValidator(baseTextValidator);

        RuleForEach(c => c.Streetcode.Tags).SetValidator(tagValidator);

        // If the object's state is marked as Deleted or Updated, the Id field must not be 0,
        // because to delete or update an object, it must already exist in the database.
        // If the Id is 0, it indicates that the object has not been saved yet, and delete or update operations
        // cannot be performed on a non-existing object.
        RuleForEach(c => c.Streetcode.Tags)
            .Where(t => t.ModelState == ModelState.Deleted || t.ModelState == ModelState.Updated)
            .Must(t => t.Id != 0)
            .WithMessage(localizer["Invalid", fieldLocalizer["Id"]]);

        RuleForEach(c => c.Streetcode.Subtitles).SetValidator(baseSubtitleValidator);
        RuleForEach(c => c.Streetcode.Facts).SetValidator(baseFactValidator);
        RuleForEach(c => c.Streetcode.StreetcodeCategoryContents).SetValidator(categoryContentValidator);
    }

    private async Task<bool> BeUniqueIndex(StreetcodeUpdateDTO streetcode, CancellationToken cancellationToken)
    {
        var existingStreetcodeByIndex = await _repositoryWrapper.StreetcodeRepository.GetFirstOrDefaultAsync(n => n.Index == streetcode.Index);
        if (existingStreetcodeByIndex != null)
        {
            return existingStreetcodeByIndex.Id == streetcode.Id;
        }

        return true;
    }
}