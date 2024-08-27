using FluentValidation;
using Streetcode.BLL.DTO.Media.Video;
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

    public BaseVideoValidator()
    {
        RuleFor(dto => dto.Url)
            .MustBeValidUrl(VideoHosts).WithName("Video Url");
    }
}