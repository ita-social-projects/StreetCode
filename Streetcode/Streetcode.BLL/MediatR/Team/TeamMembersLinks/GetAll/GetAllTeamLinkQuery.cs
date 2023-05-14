using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Team;

namespace Streetcode.BLL.MediatR.Team.TeamMembersLinks.GetAll
{
    public record GetAllTeamLinkQuery : IRequest<Result<IEnumerable<TeamMemberLinkDTO>>>;
}
