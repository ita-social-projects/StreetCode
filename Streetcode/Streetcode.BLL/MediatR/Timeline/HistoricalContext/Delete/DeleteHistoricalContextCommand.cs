using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Timeline;

namespace Streetcode.BLL.MediatR.Timeline.HistoricalContext.Delete
{
    public record DeleteHistoricalContextCommand(int contextId)
        : IRequest<Result<int>>;
}