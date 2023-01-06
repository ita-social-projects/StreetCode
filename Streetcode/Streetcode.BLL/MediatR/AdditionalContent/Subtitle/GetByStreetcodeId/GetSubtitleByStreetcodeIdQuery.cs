using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.AdditionalContent.Subtitles;

namespace Streetcode.BLL.MediatR.AdditionalContent.Subtitle.GetByStreetcodeId
{
    public record GetSubtitleByStreetcodeIdQuery(int streetcodeId) : IRequest<Result<SubtitleDTO>>;
}
