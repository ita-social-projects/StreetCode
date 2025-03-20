using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.AdditionalContent.Subtitles;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.MediatR.AdditionalContent.Subtitle.GetByStreetcodeId
{
    public record GetSubtitlesByStreetcodeIdQuery(int StreetcodeId, UserRole? UserRole)
        : IRequest<Result<SubtitleDTO>>;
}
