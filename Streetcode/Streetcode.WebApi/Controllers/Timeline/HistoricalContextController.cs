using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.DTO.Timeline;
using Streetcode.BLL.MediatR.Timeline.HistoricalContext.Create;
using Streetcode.BLL.MediatR.Timeline.HistoricalContext.Delete;
using Streetcode.BLL.MediatR.Timeline.HistoricalContext.GetAll;
using Streetcode.BLL.MediatR.Timeline.HistoricalContext.GetById;
using Streetcode.BLL.MediatR.Timeline.HistoricalContext.GetByTitle;
using Streetcode.BLL.MediatR.Timeline.HistoricalContext.Update;
using Streetcode.DAL.Enums;

namespace Streetcode.WebApi.Controllers.Timeline
{
    public class HistoricalContextController : BaseApiController
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<HistoricalContextDTO>))]
        public async Task<IActionResult> GetAll()
        {
            return HandleResult(await Mediator.Send(new GetAllHistoricalContextQuery()));
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HistoricalContextDTO))]
        public async Task<IActionResult> GetById(int id)
        {
            return HandleResult(await Mediator.Send(new GetHistoricalContextByIdQuery(id)));
        }

        [HttpGet("{title}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HistoricalContextDTO))]
        public async Task<IActionResult> GetByTitle(string title)
        {
            return HandleResult(await Mediator.Send(new GetHistoricalContextByTitleQuery(title)));
        }

        [HttpPut]
        [Authorize(Roles = nameof(UserRole.Admin))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HistoricalContextDTO))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Update([FromBody]HistoricalContextDTO contextDto)
        {
            return HandleResult(await Mediator.Send(new UpdateHistoricalContextCommand(contextDto)));
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Delete(int id)
        {
            return HandleResult(await Mediator.Send(new DeleteHistoricalContextCommand(id)));
        }

        [HttpPost]
        [Authorize(Roles = nameof(UserRole.Admin))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HistoricalContextDTO))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Create([FromBody]HistoricalContextDTO contextDto)
        {
            return HandleResult(await Mediator.Send(new CreateHistoricalContextCommand(contextDto)));
        }
    }
}
