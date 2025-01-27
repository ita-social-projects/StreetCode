using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Event;

namespace Streetcode.BLL.MediatR.Event.Update
{
    public record UpdateEventCommand(UpdateEventDTO Event)
        : IRequest<Result<int>>;
}
