using Streetcode.BLL.DTO.Event;
using FluentResults;
using MediatR;

namespace Streetcode.BLL.MediatR.Event.GetAll
{
    public record GetAllEventsQuery(ushort? page = null, ushort? pageSize = null)
        : IRequest<Result<GetAllEventsResponseDTO>>;
}
