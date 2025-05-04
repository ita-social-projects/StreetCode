using FluentValidation;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Media.Audio;
using Streetcode.BLL.SharedResource;

namespace Streetcode.BLL.Validators.Media.Audio;

public class BaseAudioValidator : AbstractValidator<AudioFileBaseCreateDTO>
{
    public const int MaxTitleLength = 100;
    public const int MaxMimeTypeLength = 10;
    public const string Mp3Extension = "mp3";

    public BaseAudioValidator(IStringLocalizer<FailedToValidateSharedResource> localizer, IStringLocalizer<FieldNamesSharedResource> fieldLocalizer)
    {
        RuleFor(dto => dto.Title)
            .NotEmpty().WithMessage(localizer[ValidationMessageConstants.IsRequired, fieldLocalizer["Title"]])
            .MaximumLength(MaxTitleLength).WithMessage(localizer["MaxLength", fieldLocalizer["Title"], MaxTitleLength]);
        RuleFor(dto => dto.BaseFormat)
            .NotEmpty().WithMessage(localizer[ValidationMessageConstants.IsRequired, fieldLocalizer["BaseFormat"]]);
        RuleFor(dto => dto.MimeType)
            .NotEmpty().WithMessage(localizer[ValidationMessageConstants.IsRequired, fieldLocalizer["MimeType"]])
            .MaximumLength(MaxMimeTypeLength).WithMessage(localizer["MaxLength", fieldLocalizer["MimeType"], MaxMimeTypeLength]);
        RuleFor(dto => dto.Extension)
            .NotEmpty().WithMessage(localizer[ValidationMessageConstants.IsRequired, fieldLocalizer["Extension"]])
            .Equal(Mp3Extension).WithMessage(localizer["MustBeOneOf", fieldLocalizer["Extension"], $"'{Mp3Extension}'"]);
    }
}