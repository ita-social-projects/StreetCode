using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Event.CreateUpdate;

namespace Streetcode.BLL.MediatR.Event.Create
{
    public record CreateEventCommand(CreateUpdateEventDto Event)
        : IRequest<Result<int>>;
}
