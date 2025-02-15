using Streetcode.BLL.DTO.Event;
using FluentResults;
using MediatR;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.MediatR.Event.GetAll
{
    public record GetAllEventsQuery(string? EventType, ushort? page = null, ushort? pageSize = null)
        : IRequest<Result<GetAllEventsResponseDTO>>;
}
