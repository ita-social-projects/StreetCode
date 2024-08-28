using FluentValidation;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.SharedResource;

namespace Streetcode.BLL.Validators.Media.Image;

public class BaseImageValidator : AbstractValidator<ImageFileBaseCreateDTO>
{
    private const int MaxTitleLength = 100;
    private const int MaxAltLength = 300;
    private const int MaxMimeTypeLength = 10;

    public BaseImageValidator(IStringLocalizer<FailedToValidateSharedResource> localizer)
    {
        RuleFor(dto => dto.Title)
            .MaximumLength(MaxTitleLength).WithMessage(localizer["TitleMaxLength", MaxTitleLength]);

        RuleFor(dto => dto.Alt)
            .NotNull().WithMessage(localizer["AltRequired"])
            .MaximumLength(MaxAltLength).WithMessage(localizer["MaxAltLength", MaxAltLength]);

        RuleFor(dto => dto.MimeType)
            .NotEmpty().WithMessage(localizer["MimeTypeRequired"])
            .MaximumLength(MaxMimeTypeLength).WithMessage(localizer["MimeTypeMaxLength", MaxMimeTypeLength]);
    }
}