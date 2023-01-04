using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.AdditionalContent.Subtitles;
using Streetcode.BLL.DTO.Streetcode.TextContent;

namespace Streetcode.BLL.MediatR.Subtitle.GetByStreetcodeId
{
    public record GetSubtitleByStreetcodeIdQuery(int streetcodeId) : IRequest<Result<SubtitleDTO>>;
}
