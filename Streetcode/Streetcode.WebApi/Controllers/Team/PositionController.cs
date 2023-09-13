using MediatR;
using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.MediatR.Team.Create;
using Streetcode.BLL.MediatR.Team.Position.GetAll;

namespace Streetcode.WebApi.Controllers.Team
{
    public class PositionController : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return HandleResult(await Mediator.Send(new GetAllPositionsQuery()));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWithTeamMembers()
		{
			return HandleResult(await Mediator.Send(new GetAllWithTeamMembersQuery()));
		}

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PositionDTO position)
        {
            return HandleResult(await Mediator.Send(new CreatePositionQuery(position)));
        }
    }
}
