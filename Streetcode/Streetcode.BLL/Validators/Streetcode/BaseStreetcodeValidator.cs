﻿using FluentValidation;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.Validators.Common;
using Streetcode.BLL.Validators.Streetcode.Art;
using Streetcode.BLL.Validators.Streetcode.ImageDetails;
using Streetcode.BLL.Validators.Streetcode.StreetcodeArtSlide;
using Streetcode.BLL.Validators.Streetcode.TimelineItem;
using Streetcode.BLL.Validators.Streetcode.Toponyms;

namespace Streetcode.BLL.Validators.Streetcode;

public class BaseStreetcodeValidator : AbstractValidator<StreetcodeCreateUpdateDTO>
{
    public const int FirstNameMaxLength = 50;
    public const int LastNameMaxLength = 50;
    public const int TitleMaxLength = 100;
    public const int AliasMaxLength = 33;
    public const int TeaserMaxLength = 520;
    public const int DateStringMaxLength = 100;
    public const int IndexMaxValue = 9999;
    public const int IndexMinValue = 1;
    public BaseStreetcodeValidator(
        StreetcodeToponymValidator streetcodeToponymValidator,
        TimelineItemValidator timelineItemValidator,
        ImageDetailsValidator imageDetailsValidator,
        StreetcodeArtSlideValidator streetcodeArtSlideValidator,
        ArtCreateUpdateDTOValidator artCreateUpdateDtoValidator)
    {
        RuleFor(dto => dto.FirstName)
            .MaximumLength(FirstNameMaxLength).WithMessage($"Maximum length of first name is {FirstNameMaxLength}");

        RuleFor(dto => dto.LastName)
            .MaximumLength(LastNameMaxLength).WithMessage($"Maximum length of last name is {LastNameMaxLength}");

        RuleFor(dto => dto.Alias)
            .MaximumLength(AliasMaxLength).WithMessage($"Maximum length of alias is {AliasMaxLength}");

        RuleFor(dto => dto.Teaser)
            .NotEmpty().WithMessage("Teaser cannot be empty")
            .MaximumLength(TeaserMaxLength).WithMessage($"Maximum length of teaser is {TeaserMaxLength}");

        RuleFor(dto => dto.Title)
            .NotEmpty().WithMessage("Title cannot be empty")
            .MaximumLength(TitleMaxLength).WithMessage($"Maximum length of title is {TitleMaxLength}");

        RuleFor(dto => dto.TransliterationUrl)
            .NotEmpty().WithMessage("TransliterationUrl cannot be empty")
            .Matches(@"^([A-Za-z]|[A-Za-z0-9]|-)*$")
            .WithMessage("TransliterationUrl must consists of latin characters, numbers and hyphen");

        RuleFor(dto => dto.Index)
            .NotNull().WithMessage("Index is required")
            .InclusiveBetween(IndexMinValue, IndexMaxValue).WithMessage($"Index should be between {IndexMinValue} and {IndexMaxValue}");

        RuleFor(dto => dto.DateString)
            .NotEmpty().WithMessage("DateString cannot be empty")
            .MaximumLength(DateStringMaxLength).WithMessage($"Maximum length of date string is {DateStringMaxLength}");

        RuleFor(dto => dto.StreetcodeType)
            .NotNull().WithMessage("Streetcode type is required")
            .IsInEnum().WithMessage("Invalid streetcode type value");

        RuleFor(dto => dto.Status)
            .NotNull().WithMessage("Status is required")
            .IsInEnum().WithMessage("Invalid status value");

        RuleFor(dto => dto.EventStartOrPersonBirthDate)
            .NotNull().WithMessage("EventStartOrPersonBirthDate is required");

        RuleForEach(dto => dto.Toponyms)
            .SetValidator(streetcodeToponymValidator);

        RuleForEach(dto => dto.TimelineItems)
            .SetValidator(timelineItemValidator);

        RuleForEach(dto => dto.ImagesDetails)
            .SetValidator(imageDetailsValidator);

        RuleForEach(dto => dto.StreetcodeArtSlides)
            .SetValidator(streetcodeArtSlideValidator);

        RuleForEach(dto => dto.Arts)
            .SetValidator(artCreateUpdateDtoValidator);
    }
}