using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Timeline;

namespace Streetcode.BLL.MediatR.Timeline.HistoricalContext.GetAll
{
    public record GetAllHistoricalContextQuery(ushort? page = null, ushort? pageSize = null, string? title = null)
        : IRequest<Result<GetAllHistoricalContextDTO>>;
}
