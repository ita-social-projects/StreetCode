using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.AdditionalContent.Subtitles;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.MediatR.AdditionalContent.GetById;

public record GetSubtitleByIdQuery(int Id, UserRole? UserRole)
    : IRequest<Result<SubtitleDTO>>;
