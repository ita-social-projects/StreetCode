using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.AdditionalContent.Subtitles;

namespace Streetcode.BLL.MediatR.Subtitle.GetById;

public record GetSubtitleByIdQuery(int Id) : IRequest<Result<SubtitleDTO>>;
