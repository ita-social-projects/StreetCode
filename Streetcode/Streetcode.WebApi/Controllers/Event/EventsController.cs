using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.DTO.Event;
using Streetcode.BLL.DTO.Event.CreateUpdate;
using Streetcode.BLL.MediatR.Event.Create;
using Streetcode.BLL.MediatR.Event.Delete;
using Streetcode.BLL.MediatR.Event.GetAll;
using Streetcode.BLL.MediatR.Event.GetById;
using Streetcode.BLL.MediatR.Event.Update;
using Streetcode.DAL.Enums;

namespace Streetcode.WebApi.Controllers.Event
{
    public class EventsController : BaseApiController
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetAllEventsResponseDTO))]
        public async Task<IActionResult> GetAll([FromQuery] ushort? page, [FromQuery] ushort? pageSize)
        {
            return HandleResult(await Mediator.Send(new GetAllEventsQuery(page, pageSize)));
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EventDTO))]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            return HandleResult(await Mediator.Send(new GetEventByIdQuery(id)));
        }

        [HttpPost]
        [Authorize(Roles = nameof(UserRole.Admin))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Create([FromBody] CreateUpdateEventDTO eventDto)
        {
            return HandleResult(await Mediator.Send(new CreateEventCommand(eventDto)));
        }

        [HttpPut]
        [Authorize(Roles = nameof(UserRole.Admin))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Update([FromBody] UpdateEventDTO eventDto)
        {
            return HandleResult(await Mediator.Send(new UpdateEventCommand(eventDto)));
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EventDTO))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            return HandleResult(await Mediator.Send(new DeleteEventCommand(id)));
        }
    }
}
