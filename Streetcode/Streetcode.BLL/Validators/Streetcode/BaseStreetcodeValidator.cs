using FluentValidation;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.SharedResource;
using Streetcode.BLL.Validators.Common;
using Streetcode.BLL.Validators.Streetcode.Art;
using Streetcode.BLL.Validators.Streetcode.ImageDetails;
using Streetcode.BLL.Validators.Streetcode.StreetcodeArtSlide;
using Streetcode.BLL.Validators.Streetcode.TimelineItem;
using Streetcode.BLL.Validators.Streetcode.Toponyms;
using Streetcode.DAL.Enums;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.Validators.Streetcode;

public class BaseStreetcodeValidator : AbstractValidator<StreetcodeCreateUpdateDTO>
{
    public const int FirstNameMaxLength = 50;
    public const int LastNameMaxLength = 50;
    public const int TitleMaxLength = 100;
    public const int AliasMaxLength = 33;
    public const int TeaserMaxLength = 520;
    public const int DateStringMaxLength = 100;
    public const int TransliterationUrlMaxLength = 100;
    public const int IndexMaxValue = 9999;
    public const int IndexMinValue = 1;
    public const int NewLineLenght = 65;

    public BaseStreetcodeValidator(
        StreetcodeToponymValidator streetcodeToponymValidator,
        TimelineItemValidator timelineItemValidator,
        ImageDetailsValidator imageDetailsValidator,
        StreetcodeArtSlideValidator streetcodeArtSlideValidator,
        ArtCreateUpdateDTOValidator artCreateUpdateDtoValidator,
        IStringLocalizer<FailedToValidateSharedResource> localizer,
        IStringLocalizer<FieldNamesSharedResource> fieldLocalizer)
    {
        RuleFor(dto => dto.FirstName)
            .MaximumLength(FirstNameMaxLength).WithMessage(localizer["MaxLength", fieldLocalizer["FirstName"], FirstNameMaxLength]);

        RuleFor(dto => dto.LastName)
            .MaximumLength(LastNameMaxLength).WithMessage(localizer["MaxLength", fieldLocalizer["LastName"], LastNameMaxLength]);

        RuleFor(dto => dto.Alias)
            .MaximumLength(AliasMaxLength).WithMessage(localizer["MaxLength", fieldLocalizer["Alias"], AliasMaxLength]);

        RuleFor(dto => dto.Teaser)
            .NotEmpty().WithMessage(localizer["CannotBeEmpty", fieldLocalizer["Teaser"]])
            .Must(AdjustedLengthIsValid).WithMessage(localizer["MaxLength", fieldLocalizer["Teaser"], TeaserMaxLength]);

        RuleFor(dto => dto.Title)
            .NotEmpty().WithMessage(localizer["CannotBeEmpty", fieldLocalizer["Title"]])
            .MaximumLength(TitleMaxLength).WithMessage(localizer["MaxLength", fieldLocalizer["Title"], TitleMaxLength]);

        RuleFor(dto => dto.TransliterationUrl)
            .NotEmpty().WithMessage(localizer["CannotBeEmpty", fieldLocalizer["TransliterationUrl"]])
            .MaximumLength(TransliterationUrlMaxLength).WithMessage(localizer["MaxLength", fieldLocalizer["TransliterationUrl"], TransliterationUrlMaxLength])
            .Matches(@"^[a-z0-9-]*$")
            .WithMessage(localizer["TransliterationUrlFormat"]);

        RuleFor(dto => dto.Index)
            .NotNull().WithMessage(localizer["IsRequired", fieldLocalizer["Index"]])
            .InclusiveBetween(IndexMinValue, IndexMaxValue).WithMessage(localizer["MustBeBetween", fieldLocalizer["Index"], IndexMinValue, IndexMaxValue]);

        RuleFor(dto => dto.DateString)
            .MaximumLength(DateStringMaxLength).WithMessage(localizer["MaxLength", fieldLocalizer["DateString"], DateStringMaxLength])
            .Matches(@"^[А-Яа-яЁёЇїІіЄєҐґ0-9\s\(\)\-\–]+$") // Cyrillic letters, digits, parentheses, hyphen
            .WithMessage(localizer["DateStringFormat"])
            .When(dto => !string.IsNullOrEmpty(dto.DateString), ApplyConditionTo.CurrentValidator);

        RuleFor(dto => dto.StreetcodeType)
            .NotNull().WithMessage(localizer["IsRequired", fieldLocalizer["StreetcodeType"]])
            .IsInEnum().WithMessage(localizer["Invalid", fieldLocalizer["StreetcodeType"]]);

        RuleFor(dto => dto)
            .Must(dto => string.IsNullOrEmpty(dto.FirstName) && string.IsNullOrEmpty(dto.LastName))
            .When(dto => dto.StreetcodeType == StreetcodeType.Event)
            .WithMessage(localizer["EventStreetcodeCannotHasFirstName"]);

        RuleFor(dto => dto.Status)
            .NotNull().WithMessage(localizer["IsRequired", fieldLocalizer["Status"]])
            .IsInEnum().WithMessage(localizer["Invalid", fieldLocalizer["Status"]]);

        RuleForEach(dto => dto.Toponyms)
            .SetValidator(streetcodeToponymValidator);

        RuleForEach(dto => dto.TimelineItems)
            .SetValidator(timelineItemValidator);

        RuleFor(dto => dto.ImagesDetails)
            .NotEmpty().WithMessage(localizer["CannotBeEmpty", fieldLocalizer["ImagesDetails"]]);

        RuleForEach(dto => dto.ImagesDetails)
            .SetValidator(imageDetailsValidator);

        // Validate that exactly one Black and White (Alt = "1") image exists
        RuleFor(dto => dto.ImagesDetails)
            .Must(images => images is not null && images.Count(i => i.Alt == $"{(int)ImageAssigment.Blackandwhite}") == 1)
            .WithMessage(localizer["MustContainExactlyOneAlt1", fieldLocalizer["Alt"]]);

        // Validate that at most one Colored (Alt = "0") image exists, if any
        RuleFor(dto => dto.ImagesDetails)
            .Must(images => images is null || images.Count(i => i.Alt == $"{(int)ImageAssigment.Animation}") <= 1)
            .WithMessage(localizer["MustContainAtMostOneAlt0", fieldLocalizer["Alt"]]);

        // Validate that at most one Optional (Alt = "2") image exists, if any
        RuleFor(dto => dto.ImagesDetails)
            .Must(images => images is null || images.Count(i => i.Alt == $"{(int)ImageAssigment.Relatedfigure}") <= 1)
            .WithMessage(localizer["MustContainAtMostOneAlt2", fieldLocalizer["Alt"]]);

        RuleForEach(dto => dto.StreetcodeArtSlides)
            .SetValidator(streetcodeArtSlideValidator);

        RuleForEach(dto => dto.Arts)
            .SetValidator(artCreateUpdateDtoValidator);
    }

    private int CountNewLines(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return 0;
        }

        return text.Count(c => c == '\n');
    }

    private bool AdjustedLengthIsValid(string teaser)
    {
        int newLineCount = CountNewLines(teaser);

        int adjustedLength = teaser.Length - newLineCount + (newLineCount * NewLineLenght);
        return adjustedLength <= TeaserMaxLength;
    }
}