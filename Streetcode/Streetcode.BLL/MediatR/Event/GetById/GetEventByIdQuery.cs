using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Event;

namespace Streetcode.BLL.MediatR.Event.GetById
{
    public record GetEventByIdQuery(int id)
        : IRequest<Result<EventDTO>>;
}
