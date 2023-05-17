using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.DTO.Team;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Team.GetAll
{
    public class GetAllMainTeamHandler : IRequestHandler<GetAllMainTeamQuery, Result<IEnumerable<TeamMemberDTO>>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;

        public GetAllMainTeamHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<TeamMemberDTO>>> Handle(GetAllMainTeamQuery request, CancellationToken cancellationToken)
        {
            var team = await _repositoryWrapper
                .TeamRepository
                .GetAllAsync(include: x => x.Where(x => x.IsMain).Include(x => x.Positions).Include(x => x.TeamMemberLinks));

            if (team is null)
            {
                return Result.Fail(new Error($"Cannot find any team"));
            }

            var teamDtos = _mapper.Map<IEnumerable<TeamMemberDTO>>(team);
            return Result.Ok(teamDtos);
        }
    }
}