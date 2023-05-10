using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.MediatR.Team.Create;
using Streetcode.BLL.MediatR.Team.GetAll;
using Streetcode.BLL.MediatR.Team.GetById;

namespace Streetcode.WebApi.Controllers.Team
{
    public class TeamController : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return HandleResult(await Mediator.Send(new GetAllTeamQuery()));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            return HandleResult(await Mediator.Send(new GetByIdTeamQuery(id)));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TeamMemberDTO teamMember)
        {
            return HandleResult(await Mediator.Send(new CreateTeamQuery(teamMember)));
        }
    }
}
