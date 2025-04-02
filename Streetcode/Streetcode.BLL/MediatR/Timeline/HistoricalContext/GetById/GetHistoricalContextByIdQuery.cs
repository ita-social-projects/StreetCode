using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Timeline;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.MediatR.Timeline.HistoricalContext.GetById;

public record GetHistoricalContextByIdQuery(int ContextId, UserRole? UserRole)
    : IRequest<Result<HistoricalContextDTO>>;