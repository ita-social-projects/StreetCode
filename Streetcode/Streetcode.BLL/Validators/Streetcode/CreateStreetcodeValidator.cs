using FluentValidation;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Streetcode.Create;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.Create;
using Streetcode.BLL.SharedResource;
using Streetcode.BLL.Validators.AdditionalContent.Tag;
using Streetcode.BLL.Validators.Common;
using Streetcode.BLL.Validators.Streetcode.CategoryContent;
using Streetcode.BLL.Validators.Streetcode.Facts;
using Streetcode.BLL.Validators.Streetcode.Subtitles;
using Streetcode.BLL.Validators.Streetcode.Text;
using Streetcode.BLL.Validators.Streetcode.Video;

namespace Streetcode.BLL.Validators.Streetcode;

public class CreateStreetcodeValidator : AbstractValidator<CreateStreetcodeCommand>
{
    public CreateStreetcodeValidator(
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
        RuleFor(c => c.Streetcode).SetValidator(baseStreetcodeValidator);
        RuleFor(c => c.Streetcode.ARBlockURL)
            .MustBeValidUrl().When(c => c.Streetcode.ARBlockURL is not null)
            .WithMessage(localizer["ValidUrl", fieldLocalizer["ARBlockURL"]]);

        RuleFor(c => c.Streetcode.Text!.Title)
            .NotEmpty()
            .When(c => c.Streetcode.Videos is not null && c.Streetcode.Text is not null && c.Streetcode.Videos.Any())
            .WithMessage(localizer["CannotBeEmptyWithCondition", fieldLocalizer["Title"], fieldLocalizer["Video"]]);
        RuleForEach(c => c.Streetcode.Videos)
            .SetValidator(videoValidator);

        RuleFor(c => c.Streetcode.Text).SetValidator(baseTextValidator);
        RuleForEach(c => c.Streetcode.Tags).SetValidator(tagValidator);
        RuleForEach(c => c.Streetcode.Subtitles).SetValidator(baseSubtitleValidator);
        RuleForEach(c => c.Streetcode.Facts).SetValidator(baseFactValidator);
        RuleForEach(c => c.Streetcode.StreetcodeCategoryContents).SetValidator(categoryContentValidator);
    }
}