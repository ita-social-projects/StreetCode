using FluentValidation;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Media.Video;
using Streetcode.BLL.SharedResource;
using Streetcode.BLL.Validators.Common;

namespace Streetcode.BLL.Validators.Streetcode.Video;

public class BaseVideoValidator : AbstractValidator<VideoCreateUpdateDTO>
{
    public static readonly List<string> VideoHosts = new()
    {
        "www.youtube.com",
        "youtube.com",
        "youtu.be"
    };

    public BaseVideoValidator(IStringLocalizer<FailedToValidateSharedResource> localizer, IStringLocalizer<FieldNamesSharedResource> fieldLocalizer)
    {
        RuleFor(dto => dto.Url)
            .NotEmpty().WithMessage(localizer["CannotBeEmpty", fieldLocalizer["Video"]])
            .MustBeValidUrl(VideoHosts).WithMessage(localizer["ValidUrl", fieldLocalizer["Video"]]);
    }
}