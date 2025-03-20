using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.AdditionalContent.Subtitles;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.MediatR.AdditionalContent.Subtitle.GetAll;

public record GetAllSubtitlesQuery(UserRole? UserRole) : IRequest<Result<IEnumerable<SubtitleDTO>>>;