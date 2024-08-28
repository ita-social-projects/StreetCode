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

    public BaseAudioValidator(IStringLocalizer<FailedToValidateSharedResource> localizer)
    {
        RuleFor(dto => dto.Title)
            .NotEmpty().WithMessage(localizer["TitleRequired"])
            .MaximumLength(MaxTitleLength).WithMessage(localizer["TitleMaxLength", MaxTitleLength]);
        RuleFor(dto => dto.MimeType)
            .NotEmpty().WithMessage(localizer["MimeTypeRequired"])
            .MaximumLength(MaxMimeTypeLength).WithMessage(localizer["MimeTypeMaxLength", MaxMimeTypeLength]);
        RuleFor(dto => dto.Extension)
            .NotEmpty().WithMessage(localizer["ExtensionRequired"])
            .Equal(Mp3Extension).WithMessage(localizer["ExtensionMustBeMp3"]);
    }
}