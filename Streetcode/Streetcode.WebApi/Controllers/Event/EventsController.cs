using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.DTO.Event;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.MediatR.Event.GetAll;
using Streetcode.BLL.MediatR.Event.GetById;
using Streetcode.BLL.MediatR.Partners.GetAll;
using Streetcode.BLL.MediatR.Partners.GetById;

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
    }
}
