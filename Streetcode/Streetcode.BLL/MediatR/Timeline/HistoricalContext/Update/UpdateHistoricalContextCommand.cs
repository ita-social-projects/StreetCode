using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Timeline;

namespace Streetcode.BLL.MediatR.Timeline.HistoricalContext.Update
{
    public record UpdateHistoricalContextCommand(HistoricalContextDTO HistoricalContext)
        : IRequest<Result<HistoricalContextDTO>>;
}