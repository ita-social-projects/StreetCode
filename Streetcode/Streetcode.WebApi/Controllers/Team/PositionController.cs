using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.MediatR.Team.Create;
using Streetcode.BLL.MediatR.Team.Position.Delete;
using Streetcode.BLL.MediatR.Team.Position.GetAll;
using Streetcode.BLL.MediatR.Team.Position.GetById;
using Streetcode.BLL.MediatR.Team.Position.GetByTitle;
using Streetcode.BLL.MediatR.Team.Position.Update;
using Streetcode.DAL.Enums;

namespace Streetcode.WebApi.Controllers.Team
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PositionController : BaseApiController
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetAllPositionsDTO))]
        public async Task<IActionResult> GetAll([FromQuery] ushort? page, [FromQuery] ushort? pageSize)
        {
            return HandleResult(await Mediator.Send(new GetAllPositionsQuery(page, pageSize)));
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<PositionDTO>))]
        public async Task<IActionResult> GetAllWithTeamMembers()
		{
			return HandleResult(await Mediator.Send(new GetAllWithTeamMembersQuery()));
		}

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PositionDTO))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Create([FromBody] PositionCreateDTO position)
        {
            return HandleResult(await Mediator.Send(new CreatePositionQuery(position)));
        }

        [HttpPut]
        [Authorize(Roles = nameof(UserRole.Admin))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PositionDTO))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Update([FromBody]PositionDTO contextDto)
        {
            return HandleResult(await Mediator.Send(new UpdateTeamPositionCommand(contextDto)));
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Delete(int id)
        {
            return HandleResult(await Mediator.Send(new DeleteTeamPositionCommand(id)));
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PositionDTO))]
        public async Task<IActionResult> GetById(int id)
        {
            return HandleResult(await Mediator.Send(new GetByIdTeamPositionQuery(id)));
        }

        [HttpGet("{title}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PositionDTO))]
        public async Task<IActionResult> GetByTitle(string title)
        {
            return HandleResult(await Mediator.Send(new GetByTitleTeamPositionQuery(title)));
        }
    }
}
