using FluentValidation;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.SharedResource;
using Streetcode.BLL.Util.Extensions;
using Streetcode.BLL.Validators.Common;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.Validators.Media.Image;

public class BaseImageValidator : AbstractValidator<ImageFileBaseCreateDTO>
{
    public const int MaxTitleLength = 100;
    public const int MaxAltLength = 300;
    public const int MaxMimeTypeLength = 10;
    private const int MaxImageSizeInMb = 3;
    private readonly List<string> _extensions = new() { "png", "jpeg", "jpg", "webp" };
    private readonly List<string> _mimeTypes = new() { "image/jpeg", "image/png", "image/webp" };

    public BaseImageValidator(IStringLocalizer<FailedToValidateSharedResource> localizer, IStringLocalizer<FieldNamesSharedResource> fieldLocalizer)
    {
        RuleFor(dto => dto.Title)
            .NotEmpty().WithMessage(localizer[ValidationMessageConstants.IsRequired, fieldLocalizer["Title"]])
            .MaximumLength(MaxTitleLength).WithMessage(localizer["MaxLength", fieldLocalizer["Title"], MaxTitleLength]);

        RuleFor(dto => dto.Alt)
            .Must(value => Enum.TryParse<ImageAssigment>(value, out var parsedValue) &&
                           parsedValue >= EnumExtensions.Min<ImageAssigment>() &&
                           parsedValue <= EnumExtensions.Max<ImageAssigment>())
            .WithMessage(localizer["MustBeBetween", fieldLocalizer["Alt"], (int)EnumExtensions.Min<ImageAssigment>(), (int)EnumExtensions.Max<ImageAssigment>()])
            .MaximumLength(MaxAltLength)
            .WithMessage(localizer["MaxLength", fieldLocalizer["Alt"], MaxAltLength]);

        RuleFor(dto => dto.BaseFormat)
            .NotEmpty().WithMessage(localizer[ValidationMessageConstants.IsRequired, fieldLocalizer["BaseFormat"]])
            .Must(IsImageSizeValid).WithMessage(localizer["ImageSizeExceeded", MaxImageSizeInMb]);

        RuleFor(dto => dto.MimeType)
            .NotEmpty().WithMessage(localizer[ValidationMessageConstants.IsRequired, fieldLocalizer["MimeType"]])
            .MaximumLength(MaxMimeTypeLength)
            .WithMessage(localizer["MaxLength", fieldLocalizer["MimeType"], MaxMimeTypeLength])
            .Must(x => _mimeTypes.Contains(x.ToLower())).WithMessage(localizer["MustBeOneOf", fieldLocalizer["MimeType"], ValidationExtentions.ConcatWithComma(_mimeTypes)]);

        RuleFor(dto => dto.Extension)
            .NotEmpty().WithMessage(localizer[ValidationMessageConstants.IsRequired, fieldLocalizer["Extension"]])
            .Must(x => _extensions.Contains(x.ToLower()))
            .WithMessage(localizer["MustBeOneOf", fieldLocalizer["Extension"], ValidationExtentions.ConcatWithComma(_extensions)]);
    }

    private static bool IsImageSizeValid(string baseFormat)
    {
        int paddingCount = baseFormat.EndsWith("==") ? 2 :
            baseFormat.EndsWith('=') ? 1 : 0;
        int sizeInBytes = (baseFormat.Length * 3 / 4) - paddingCount;
        int maxFileSizeInBytes = MaxImageSizeInMb * 1024 * 1024;
        return sizeInBytes <= maxFileSizeInBytes;
    }
}