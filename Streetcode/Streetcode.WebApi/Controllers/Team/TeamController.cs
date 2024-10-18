﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.MediatR.Team.Create;
using Streetcode.BLL.MediatR.Team.Delete;
using Streetcode.BLL.MediatR.Team.GetAll;
using Streetcode.BLL.MediatR.Team.GetById;
using Streetcode.BLL.MediatR.Team.GetByRoleId;
using Streetcode.BLL.MediatR.Team.Update;
using Streetcode.DAL.Enums;

namespace Streetcode.WebApi.Controllers.Team
{
    public class TeamController : BaseApiController
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetAllTeamDTO))]
        public async Task<IActionResult> GetAll([FromQuery] ushort? page, [FromQuery] ushort? pageSize)
        {
            return HandleResult(await Mediator.Send(new GetAllTeamQuery(page, pageSize)));
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TeamMemberDTO>))]
        public async Task<IActionResult> GetAllMain()
        {
            return HandleResult(await Mediator.Send(new GetAllMainTeamQuery()));
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TeamMemberDTO))]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            return HandleResult(await Mediator.Send(new GetByIdTeamQuery(id)));
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TeamMemberDTO>))]
        public async Task<IActionResult> GetByRoleId([FromRoute] int id)
		{
			return HandleResult(await Mediator.Send(new GetTeamByRoleIdQuery(id)));
		}

        [HttpPost]
        [Authorize(Roles = nameof(UserRole.Admin))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TeamMemberDTO))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Create([FromBody] TeamMemberCreateDTO teamMember)
        {
            return HandleResult(await Mediator.Send(new CreateTeamQuery(teamMember)));
        }

        [HttpPut]
        [Authorize(Roles = nameof(UserRole.Admin))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UpdateTeamMemberDTO))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Update([FromBody] UpdateTeamMemberDTO teamMember)
        {
            return HandleResult(await Mediator.Send(new UpdateTeamQuery(teamMember)));
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TeamMemberDTO))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            return HandleResult(await Mediator.Send(new DeleteTeamQuery(id)));
        }
    }
}
