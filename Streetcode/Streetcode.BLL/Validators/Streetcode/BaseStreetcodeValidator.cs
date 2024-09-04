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
    public const int IndexMaxValue = 9999;
    public const int IndexMinValue = 1;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public BaseStreetcodeValidator(
        IRepositoryWrapper repositoryWrapper,
        StreetcodeToponymValidator streetcodeToponymValidator,
        TimelineItemValidator timelineItemValidator,
        ImageDetailsValidator imageDetailsValidator,
        StreetcodeArtSlideValidator streetcodeArtSlideValidator,
        ArtCreateUpdateDTOValidator artCreateUpdateDtoValidator,
        IStringLocalizer<FailedToValidateSharedResource> localizer,
        IStringLocalizer<FieldNamesSharedResource> fieldLocalizer)
    {
        _repositoryWrapper = repositoryWrapper;

        RuleFor(dto => dto.FirstName)
            .MaximumLength(FirstNameMaxLength).WithMessage(localizer["MaxLength", fieldLocalizer["FirstName"], FirstNameMaxLength]);

        RuleFor(dto => dto.LastName)
            .MaximumLength(LastNameMaxLength).WithMessage(localizer["MaxLength", fieldLocalizer["LastName"], LastNameMaxLength]);

        RuleFor(dto => dto.Alias)
            .MaximumLength(AliasMaxLength).WithMessage(localizer["MaxLength", fieldLocalizer["Alias"], AliasMaxLength]);

        RuleFor(dto => dto.Teaser)
            .NotEmpty().WithMessage(localizer["CannotBeEmpty", fieldLocalizer["Teaser"]])
            .MaximumLength(TeaserMaxLength).WithMessage(localizer["MaxLength", fieldLocalizer["Teaser"], TeaserMaxLength]);

        RuleFor(dto => dto.Title)
            .NotEmpty().WithMessage(localizer["CannotBeEmpty", fieldLocalizer["Title"]])
            .MaximumLength(TitleMaxLength).WithMessage(localizer["MaxLength", fieldLocalizer["Title"], TitleMaxLength]);

        RuleFor(dto => dto.TransliterationUrl)
            .NotEmpty().WithMessage(localizer["CannotBeEmpty", fieldLocalizer["TransliterationUrl"]])
            .Matches(@"^([A-Za-z]|[A-Za-z0-9]|-)*$")
            .WithMessage(localizer["TransliterationUrlFormat"]);

        RuleFor(dto => dto.Index)
            .NotNull().WithMessage(localizer["IsRequired", fieldLocalizer["Index"]])
            .InclusiveBetween(IndexMinValue, IndexMaxValue).WithMessage(localizer["MustBeBetween", fieldLocalizer["Index"], IndexMinValue, IndexMaxValue])
            .MustAsync(BeUniqueIndex).WithMessage(x => localizer["MustBeUnique", fieldLocalizer["Index"]]);

        RuleFor(dto => dto.DateString)
            .NotEmpty().WithMessage(localizer["CannotBeEmpty", fieldLocalizer["DateString"]])
            .MaximumLength(DateStringMaxLength).WithMessage(localizer["MaxLength", fieldLocalizer["DateString"], DateStringMaxLength]);

        RuleFor(dto => dto.StreetcodeType)
            .NotNull().WithMessage(localizer["IsRequired", fieldLocalizer["StreetcodeType"]])
            .IsInEnum().WithMessage(localizer["Invalid", fieldLocalizer["StreetcodeType"]]);

        RuleFor(dto => dto.Status)
            .NotNull().WithMessage(localizer["IsRequired", fieldLocalizer["Status"]])
            .IsInEnum().WithMessage(localizer["Invalid", fieldLocalizer["Status"]]);

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

    private async Task<bool> BeUniqueIndex(int index, CancellationToken cancellationToken)
    {
        var existingStreetcodeByIndex = await _repositoryWrapper.StreetcodeRepository.GetFirstOrDefaultAsync(n => n.Index == index);

        return existingStreetcodeByIndex is null;
    }
}