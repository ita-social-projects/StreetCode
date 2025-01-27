using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Event.CreateUpdate;

namespace Streetcode.BLL.MediatR.Event.Create
{
    public record CreateEventCommand(CreateUpdateEventDTO Event)
        : IRequest<Result<int>>;
}
