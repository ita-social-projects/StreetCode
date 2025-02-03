using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Team.Position.GetAll
{
    public class GetAllWithTeamMembersHandler : IRequestHandler<GetAllWithTeamMembersQuery, Result<IEnumerable<PositionDto>>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;
        private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizer;

        public GetAllWithTeamMembersHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger, IStringLocalizer<CannotFindSharedResource> stringLocalizer)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
            _stringLocalizer = stringLocalizer;
        }

        public async Task<Result<IEnumerable<PositionDto>>> Handle(GetAllWithTeamMembersQuery request, CancellationToken cancellationToken)
        {
            var positions = await _repositoryWrapper
                .PositionRepository
                .GetAllAsync(include: x => x.Include(x => x.TeamMembers!));

            var positionsWithMembers = positions.Where(x => x.TeamMembers!.Any());

            if (positionsWithMembers is null)
            {
                string errorMsg = _stringLocalizer["CannotFindAnyPositions"];
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            return Result.Ok(_mapper.Map<IEnumerable<PositionDto>>(positionsWithMembers));
        }
    }
}
