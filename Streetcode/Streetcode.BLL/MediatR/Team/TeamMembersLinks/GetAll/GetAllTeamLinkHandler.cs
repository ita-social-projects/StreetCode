using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Team;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Team.TeamMembersLinks.GetAll
{
    public class GetAllTeamLinkHandler : IRequestHandler<GetAllTeamLinkQuery, Result<IEnumerable<TeamMemberLinkDTO>>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;

        public GetAllTeamLinkHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<TeamMemberLinkDTO>>> Handle(GetAllTeamLinkQuery request, CancellationToken cancellationToken)
        {
            var teamLinks = await _repositoryWrapper
                .TeamLinkRepository
                .GetAllAsync();

            if (teamLinks is null)
            {
                return Result.Fail(new Error($"Cannot find any team links"));
            }

            var teamLinksDtos = _mapper.Map<IEnumerable<TeamMemberLinkDTO>>(teamLinks);
            return Result.Ok(teamLinksDtos);
        }
    }
}
