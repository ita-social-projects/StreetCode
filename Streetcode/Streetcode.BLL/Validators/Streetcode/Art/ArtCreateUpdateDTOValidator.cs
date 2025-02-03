using FluentValidation;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Media.Create;
using Streetcode.BLL.SharedResource;

namespace Streetcode.BLL.Validators.Streetcode.Art;

public class ArtCreateUpdateDtoValidator : AbstractValidator<ArtCreateUpdateDto>
{
    public const int MaxTitleLength = 150;
    public const int MaxDescriptionLength = 400;
    public ArtCreateUpdateDtoValidator(IStringLocalizer<FailedToValidateSharedResource> localizer, IStringLocalizer<FieldNamesSharedResource> fieldLocalizer)
    {
        RuleFor(dto => dto.Description)
            .MaximumLength(MaxDescriptionLength).WithMessage(localizer["MaxLength", fieldLocalizer["ArtDescription"], MaxDescriptionLength]);

        RuleFor(dto => dto.Title)
            .MaximumLength(MaxTitleLength).WithMessage(localizer["MaxLength", fieldLocalizer["ArtTitle"], MaxTitleLength]);

        RuleFor(dto => dto.ModelState)
            .IsInEnum().WithMessage(localizer["Invalid", fieldLocalizer["ModelState"]]);
    }
}