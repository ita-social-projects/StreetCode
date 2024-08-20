using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.DTO.AdditionalContent.Subtitles;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Team.GetAll;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Team.Position.GetAll
{
    public class GetAllWithTeamMembersHandler : IRequestHandler<GetAllWithTeamMembersQuery, Result<IEnumerable<PositionDTO>>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;

        public GetAllWithTeamMembersHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<PositionDTO>>> Handle(GetAllWithTeamMembersQuery request, CancellationToken cancellationToken)
        {
            var positions = await _repositoryWrapper
                .PositionRepository
                .GetAllAsync(include: x => x.Include(x => x.TeamMembers));

            var positionsWithMembers = positions.Where(x => x.TeamMembers.Any());

            if (!positionsWithMembers.Any())
            {
                const string errorMsg = $"Cannot find any positions";
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            return Result.Ok(_mapper.Map<IEnumerable<PositionDTO>>(positionsWithMembers));
        }
    }
}
