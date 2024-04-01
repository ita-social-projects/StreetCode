using FluentResults;
using MediatR;

namespace Streetcode.BLL.MediatR.Timeline.HistoricalContext.Delete
{
    public record DeleteHistoricalContextCommand(int contextId)
        : IRequest<Result<int>>;
}