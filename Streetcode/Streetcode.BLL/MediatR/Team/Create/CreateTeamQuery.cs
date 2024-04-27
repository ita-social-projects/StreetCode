using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Team;

namespace Streetcode.BLL.MediatR.Team.Create
{
    public record CreateTeamQuery(TeamMemberCreateDTO teamMember) : IRequest<Result<TeamMemberDTO>>;
}
