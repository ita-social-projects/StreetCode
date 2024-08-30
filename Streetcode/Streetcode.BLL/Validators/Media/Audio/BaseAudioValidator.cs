using FluentValidation;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Media.Audio;
using Streetcode.BLL.SharedResource;

namespace Streetcode.BLL.Validators.Media.Audio;

public class BaseAudioValidator : AbstractValidator<AudioFileBaseCreateDTO>
{
    private const int MaxTitleLength = 100;
    private const int MaxMimeTypeLength = 10;
    private const string Mp3Extension = "mp3";

    public BaseAudioValidator(IStringLocalizer<FailedToValidateSharedResource> localizer, IStringLocalizer<FieldNamesSharedResource> fieldLocalizer)
    {
        RuleFor(dto => dto.Title)
            .NotEmpty().WithMessage(localizer["IsRequired", fieldLocalizer["Title"]])
            .MaximumLength(MaxTitleLength).WithMessage(localizer["MaxLength", fieldLocalizer["Title"], MaxTitleLength]);
        RuleFor(dto => dto.BaseFormat)
            .NotEmpty().WithMessage(localizer["IsRequired", fieldLocalizer["BaseFormat"]]);
        RuleFor(dto => dto.MimeType)
            .NotEmpty().WithMessage(localizer["IsRequired", fieldLocalizer["MimeType"]])
            .MaximumLength(MaxMimeTypeLength).WithMessage(localizer["MaxLength", fieldLocalizer["MimeType"], MaxMimeTypeLength]);
        RuleFor(dto => dto.Extension)
            .NotEmpty().WithMessage(localizer["IsRequired", fieldLocalizer["Extension"]])
            .Equal(Mp3Extension).WithMessage(localizer["MustBeOneOf", fieldLocalizer["Extension"], $"'{Mp3Extension}'"]);
    }
}