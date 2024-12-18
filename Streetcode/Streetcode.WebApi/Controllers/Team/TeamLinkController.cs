using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.MediatR.Team.TeamMembersLinks.Create;
using Streetcode.BLL.MediatR.Team.TeamMembersLinks.GetAll;
using Streetcode.DAL.Enums;

namespace Streetcode.WebApi.Controllers.Team
{
    public class TeamLinkController : BaseApiController
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TeamMemberLinkDTO>))]
        public async Task<IActionResult> GetAll()
        {
            return HandleResult(await Mediator.Send(new GetAllTeamLinkQuery()));
        }

        [HttpPost]
        [Authorize(Roles = nameof(UserRole.Admin))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TeamMemberLinkDTO))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Create([FromBody] TeamMemberLinkCreateDTO teamMemberLink)
        {
            return HandleResult(await Mediator.Send(new CreateTeamLinkQuery(teamMemberLink)));
        }
    }
}
