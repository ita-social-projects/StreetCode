using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.AdditionalContent.Subtitles;
using Streetcode.BLL.DTO.Streetcode.TextContent;

namespace Streetcode.BLL.MediatR.Subtitle.GetAll;

public record GetAllSubtitlesQuery : IRequest<Result<IEnumerable<SubtitleDTO>>>;