using FluentValidation;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Media.Create;
using Streetcode.BLL.SharedResource;

namespace Streetcode.BLL.Validators.Streetcode.Art;

public class ArtCreateUpdateDTOValidator : AbstractValidator<ArtCreateUpdateDTO>
{
    private const int MaxTitleLength = 150;
    private const int MaxDescriptionLength = 400;
    public ArtCreateUpdateDTOValidator(IStringLocalizer<FailedToValidateSharedResource> localizer)
    {
        RuleFor(dto => dto.Description)
            .MaximumLength(MaxDescriptionLength).WithMessage(localizer["DescriptionMaxLength", MaxDescriptionLength]);

        RuleFor(dto => dto.Title)
            .MaximumLength(MaxTitleLength).WithMessage(localizer["TitleMaxLength", MaxTitleLength]);

        RuleFor(dto => dto.ModelState)
            .IsInEnum().WithMessage(localizer["InvalidModelState"]);
    }
}