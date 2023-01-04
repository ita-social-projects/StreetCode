using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.AdditionalContent.Subtitles;

namespace Streetcode.BLL.MediatR.Subtitle.GetByStreetcodeId
{
    public record GetSubtitleByStreetcodeIdQuery(int streetcodeId) : IRequest<Result<SubtitleDTO>>;
}
