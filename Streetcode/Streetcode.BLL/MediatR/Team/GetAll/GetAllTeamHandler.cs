using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Team.GetAll
{
    public class GetAllTeamHandler : IRequestHandler<GetAllTeamQuery, Result<IEnumerable<TeamMemberDTO>>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService? _logger;

        public GetAllTeamHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService? logger = null)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<TeamMemberDTO>>> Handle(GetAllTeamQuery request, CancellationToken cancellationToken)
        {
            var team = await _repositoryWrapper
                .TeamRepository
                .GetAllAsync(include: x => x.Include(x => x.Positions).Include(x => x.TeamMemberLinks));

            if (team is null)
            {
                const string errorMsg = $"Cannot find any team";
                _logger?.LogError("GetAllTeamQuery handled with an error");
                _logger?.LogError(errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            var teamDtos = _mapper.Map<IEnumerable<TeamMemberDTO>>(team);
            _logger?.LogInformation($"GetAllTeamQuery handled successfully");
            return Result.Ok(teamDtos);
        }
    }
}