using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Team.GetAll
{
    public class GetAllMainTeamHandler : IRequestHandler<GetAllMainTeamQuery, Result<IEnumerable<TeamMemberDTO>>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;
        private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;

        public GetAllMainTeamHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger, IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
            _stringLocalizerCannotFind = stringLocalizerCannotFind;
        }

        public async Task<Result<IEnumerable<TeamMemberDTO>>> Handle(GetAllMainTeamQuery request, CancellationToken cancellationToken)
        {
            var team = await _repositoryWrapper
                .TeamRepository
                .GetAllAsync(include: x => x.Where(x => x.IsMain).Include(x => x.Positions).Include(x => x.TeamMemberLinks));

            if (team is null)
            {
                string errorMsg = _stringLocalizerCannotFind["CannotFindAnyTeam"].Value;
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            return Result.Ok(_mapper.Map<IEnumerable<TeamMemberDTO>>(team));
        }
    }
}