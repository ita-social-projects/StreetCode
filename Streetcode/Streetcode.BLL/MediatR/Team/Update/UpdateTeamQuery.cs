using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.DTO.Team;

namespace Streetcode.BLL.MediatR.Team.Update
{
    public record UpdateTeamQuery(TeamMemberDTO TeamMember) : IRequest<Result<UpdateTeamMemberDTO>>;
}
