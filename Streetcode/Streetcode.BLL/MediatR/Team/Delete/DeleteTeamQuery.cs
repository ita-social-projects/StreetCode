using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Team;

namespace Streetcode.BLL.MediatR.Team.Delete
{
    public record DeleteTeamQuery(int id)
        : IRequest<Result<TeamMemberDto>>;
}