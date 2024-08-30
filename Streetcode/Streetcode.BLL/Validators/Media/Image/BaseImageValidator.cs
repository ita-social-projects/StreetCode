using FluentValidation;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.SharedResource;
using Streetcode.BLL.Validators.Common;

namespace Streetcode.BLL.Validators.Media.Image;

public class BaseImageValidator : AbstractValidator<ImageFileBaseCreateDTO>
{
    private const int MaxTitleLength = 100;
    private const int MaxAltLength = 300;
    private const int MaxMimeTypeLength = 10;

    private readonly List<string> _extensions = new() { "png", "jpeg", "jpg", "webp" };
    private readonly List<string> _mimeTypes = new() { "image/jpeg", "image/png", "image/webp" };
    public BaseImageValidator(IStringLocalizer<FailedToValidateSharedResource> localizer, IStringLocalizer<FieldNamesSharedResource> fieldLocalizer)
    {
        RuleFor(dto => dto.Title)
            .NotEmpty().WithMessage(localizer["IsRequired", fieldLocalizer["Title"]])
            .MaximumLength(MaxTitleLength).WithMessage(localizer["MaxLength", fieldLocalizer["Title"], MaxTitleLength]);

        RuleFor(dto => dto.Alt)
            .NotEmpty().WithMessage(localizer["IsRequired", fieldLocalizer["Alt"]])
            .MaximumLength(MaxAltLength).WithMessage(localizer["MaxLength", fieldLocalizer["Alt"], MaxAltLength]);

        RuleFor(dto => dto.BaseFormat)
            .NotEmpty().WithMessage(localizer["IsRequired", fieldLocalizer["BaseFormat"]]);

        RuleFor(dto => dto.MimeType)
            .NotEmpty().WithMessage(localizer["IsRequired", fieldLocalizer["MimeType"]])
            .MaximumLength(MaxMimeTypeLength)
            .WithMessage(localizer["MaxLength", fieldLocalizer["MimeType"], MaxMimeTypeLength])
            .Must(x => _mimeTypes.Contains(x)).WithMessage(localizer["MustBeOneOf", fieldLocalizer["MimeType"], ValidationExtentions.ConcatWithComma(_mimeTypes)]);

        RuleFor(dto => dto.Extension).Must(x => _extensions.Contains(x))
            .WithMessage(localizer["MustBeOneOf", fieldLocalizer["Extension"], ValidationExtentions.ConcatWithComma(_extensions)]);
    }
}