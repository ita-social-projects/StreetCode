using FluentValidation;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.SharedResource;

namespace Streetcode.BLL.Validators.Streetcode.ImageDetails;

public class ImageDetailsValidator : AbstractValidator<ImageDetailsDto>
{
    private const int TitleMaxLength = 100;
    private const int AltMaxLength = 200;
    public ImageDetailsValidator(IStringLocalizer<FailedToValidateSharedResource> localizer, IStringLocalizer<FieldNamesSharedResource> fieldLocalizer)
    {
        RuleFor(dto => dto.Title)
            .MaximumLength(TitleMaxLength).WithMessage(localizer["MaxLength", fieldLocalizer["ImageTitle"], TitleMaxLength]);
        RuleFor(dto => dto.Alt)
            .MaximumLength(AltMaxLength).WithMessage(localizer["MaxLength", fieldLocalizer["Alt"], AltMaxLength]);
    }
}