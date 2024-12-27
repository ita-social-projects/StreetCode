using FluentValidation;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.SharedResource;
using Streetcode.BLL.Util;
using Streetcode.BLL.Validators.Common;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.Validators.Media.Image;

public class BaseImageValidator : AbstractValidator<ImageFileBaseCreateDTO>
{
    public const int MaxTitleLength = 100;
    public const int MaxAltLength = 300;
    public const int MaxMimeTypeLength = 10;

    public readonly List<string> Extensions = new() { "png", "jpeg", "jpg", "webp" };
    public readonly List<string> MimeTypes = new() { "image/jpeg", "image/png", "image/webp" };
    public BaseImageValidator(IStringLocalizer<FailedToValidateSharedResource> localizer, IStringLocalizer<FieldNamesSharedResource> fieldLocalizer)
    {
        RuleFor(dto => dto.Title)
            .NotEmpty().WithMessage(localizer["IsRequired", fieldLocalizer["Title"]])
            .MaximumLength(MaxTitleLength).WithMessage(localizer["MaxLength", fieldLocalizer["Title"], MaxTitleLength]);

        RuleFor(dto => dto.Alt)
            .Must(value => Enum.TryParse<ImageAssigment>(value, out var parsedValue) &&
                           parsedValue >= EnumExtensions.Min<ImageAssigment>() &&
                           parsedValue <= EnumExtensions.Max<ImageAssigment>())
            .WithMessage(localizer["MustBeBetween", fieldLocalizer["Alt"], (int)EnumExtensions.Min<ImageAssigment>(), (int)EnumExtensions.Max<ImageAssigment>()])
            .MaximumLength(MaxAltLength)
            .WithMessage(localizer["MaxLength", fieldLocalizer["Alt"], MaxAltLength]);

        RuleFor(dto => dto.BaseFormat)
            .NotEmpty().WithMessage(localizer["IsRequired", fieldLocalizer["BaseFormat"]]);

        RuleFor(dto => dto.MimeType)
            .NotEmpty().WithMessage(localizer["IsRequired", fieldLocalizer["MimeType"]])
            .MaximumLength(MaxMimeTypeLength)
            .WithMessage(localizer["MaxLength", fieldLocalizer["MimeType"], MaxMimeTypeLength])
            .Must(x => MimeTypes.Contains(x.ToLower())).WithMessage(localizer["MustBeOneOf", fieldLocalizer["MimeType"], ValidationExtentions.ConcatWithComma(MimeTypes)]);

        RuleFor(dto => dto.Extension)
            .NotEmpty().WithMessage(localizer["IsRequired", fieldLocalizer["Extension"]])
            .Must(x => Extensions.Contains(x.ToLower()))
            .WithMessage(localizer["MustBeOneOf", fieldLocalizer["Extension"], ValidationExtentions.ConcatWithComma(Extensions)]);
    }
}