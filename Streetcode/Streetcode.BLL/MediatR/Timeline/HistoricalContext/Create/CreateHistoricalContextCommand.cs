using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Timeline;

namespace Streetcode.BLL.MediatR.Timeline.HistoricalContext.Create
{
    public record CreateHistoricalContextCommand(HistoricalContextDto HistoricalContext)
        : IRequest<Result<HistoricalContextDto>>;
}