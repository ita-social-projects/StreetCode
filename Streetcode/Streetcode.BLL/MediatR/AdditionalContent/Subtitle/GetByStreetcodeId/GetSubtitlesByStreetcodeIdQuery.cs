using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.AdditionalContent.Subtitles;

namespace Streetcode.BLL.MediatR.AdditionalContent.Subtitle.GetByStreetcodeId
{
    public record GetSubtitlesByStreetcodeIdQuery(int streetcodeId) : IRequest<Result<IEnumerable<SubtitleDTO>>>;
}
