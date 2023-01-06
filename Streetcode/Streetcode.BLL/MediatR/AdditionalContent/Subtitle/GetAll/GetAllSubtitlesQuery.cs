using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.AdditionalContent.Subtitles;

namespace Streetcode.BLL.MediatR.AdditionalContent.Subtitle.GetAll;

public record GetAllSubtitlesQuery : IRequest<Result<IEnumerable<SubtitleDTO>>>;