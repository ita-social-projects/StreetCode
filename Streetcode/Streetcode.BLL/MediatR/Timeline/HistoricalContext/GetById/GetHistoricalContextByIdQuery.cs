using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Timeline;

namespace Streetcode.BLL.MediatR.Timeline.HistoricalContext.GetById;

public record GetHistoricalContextByIdQuery(int contextId)
    : IRequest<Result<HistoricalContextDTO>>;