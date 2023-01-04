using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.AdditionalContent.Subtitles;

namespace Streetcode.BLL.MediatR.Subtitle.GetAll;

public record GetAllSubtitlesQuery : IRequest<Result<IEnumerable<SubtitleDTO>>>;