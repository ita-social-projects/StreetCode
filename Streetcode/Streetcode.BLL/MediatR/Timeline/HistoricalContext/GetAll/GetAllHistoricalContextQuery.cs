using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Timeline;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.MediatR.Timeline.HistoricalContext.GetAll
{
    public record GetAllHistoricalContextQuery(UserRole? UserRole, ushort? page = null, ushort? pageSize = null, string? title = null)
        : IRequest<Result<GetAllHistoricalContextDTO>>;
}
