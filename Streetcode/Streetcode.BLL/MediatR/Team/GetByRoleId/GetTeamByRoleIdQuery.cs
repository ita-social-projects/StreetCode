using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Team;

namespace Streetcode.BLL.MediatR.Team.GetByRoleId
{
	public record GetTeamByRoleIdQuery(int roleId)
		: IRequest<Result<IEnumerable<TeamMemberDTO>>>;
}
