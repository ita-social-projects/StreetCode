using FluentValidation;
using Streetcode.BLL.DTO.Media.Video;
using Streetcode.BLL.Validators.Common;

namespace Streetcode.BLL.Validators.Streetcode.Video;

public class BaseVideoValidator : AbstractValidator<VideoCreateUpdateDTO>
{
    public const string VideoHost = "youtube.com";
    public BaseVideoValidator()
    {
        RuleFor(dto => dto.Url)
            .MustBeValidUrl(VideoHost).WithName("Video Url");
    }
}