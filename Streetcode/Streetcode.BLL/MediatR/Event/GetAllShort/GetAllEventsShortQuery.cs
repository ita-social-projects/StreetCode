using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Event;

namespace Streetcode.BLL.MediatR.Event.GetAllShort
{
    public record GetAllEventsShortQuery : IRequest<Result<IEnumerable<EventShortDto>>>;
}
