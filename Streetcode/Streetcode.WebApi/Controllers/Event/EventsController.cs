using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.DTO.Event;
using Streetcode.BLL.DTO.Event.CreateUpdate;
using Streetcode.BLL.MediatR.Event.Create;
using Streetcode.BLL.MediatR.Event.Delete;
using Streetcode.BLL.MediatR.Event.GetAll;
using Streetcode.BLL.MediatR.Event.GetAllShort;
using Streetcode.BLL.MediatR.Event.GetById;
using Streetcode.BLL.MediatR.Event.Update;
using Streetcode.DAL.Enums;

namespace Streetcode.WebApi.Controllers.Event
{
    public class EventsController : BaseApiController
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetAllEventsResponseDto))]
        public async Task<IActionResult> GetAll([FromQuery] string? eventType, [FromQuery] ushort? page, [FromQuery] ushort? pageSize)
        {
            return HandleResult(await Mediator.Send(new GetAllEventsQuery(eventType, page, pageSize)));
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<EventShortDto>))]
        public async Task<IActionResult> GetAllShort()
        {
            return HandleResult(await Mediator.Send(new GetAllEventsShortQuery()));
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EventDto))]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            return HandleResult(await Mediator.Send(new GetEventByIdQuery(id)));
        }

        [HttpPost]
        [Authorize(Roles = nameof(UserRole.Admin))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Create([FromBody] CreateUpdateEventDto eventDto)
        {
            return HandleResult(await Mediator.Send(new CreateEventCommand(eventDto)));
        }

        [HttpPut]
        [Authorize(Roles = nameof(UserRole.Admin))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Update([FromBody] UpdateEventDto eventDto)
        {
            return HandleResult(await Mediator.Send(new UpdateEventCommand(eventDto)));
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EventDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            return HandleResult(await Mediator.Send(new DeleteEventCommand(id)));
        }
    }
}
