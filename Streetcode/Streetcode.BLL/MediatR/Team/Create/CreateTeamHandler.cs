using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Team;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Team.Create
{
    public class CreateTeamHandler : IRequestHandler<CreateTeamQuery, Result<TeamMemberDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repository;
        private readonly ILoggerService _logger;
        private readonly IStringLocalizer<FailedToCreateSharedResource> _stringLocalizerFailed;

        public CreateTeamHandler(
            IMapper mapper,
            IRepositoryWrapper repository,
            ILoggerService logger,
            IStringLocalizer<FailedToCreateSharedResource> stringLocalizerFailed)
        {
            _mapper = mapper;
            _repository = repository;
            _logger = logger;
            _stringLocalizerFailed = stringLocalizerFailed;
        }

        public async Task<Result<TeamMemberDTO>> Handle(CreateTeamQuery request, CancellationToken cancellationToken)
        {
            var teamMember = _mapper.Map<TeamMember>(request.teamMember);
            try
            {
                teamMember.Positions!.Clear();

                teamMember = await _repository.TeamRepository.CreateAsync(teamMember);
                await _repository.SaveChangesAsync();

                var newPositions = request.teamMember.Positions!.ToList();

                foreach (var newPosition in newPositions)
                {
                    if (newPosition.Id < 0)
                    {
                        Positions position = new () { Id = 0, Position = newPosition.Position, TeamMembers = null };
                        var tpm = await _repository.PositionRepository.CreateAsync(position);

                        await _repository.SaveChangesAsync();

                        await _repository.TeamPositionRepository.CreateAsync(
                            new TeamMemberPositions { TeamMemberId = teamMember.Id, PositionsId = tpm.Id });
                    }
                    else
                    {
                        await _repository.TeamPositionRepository.CreateAsync(
                            new TeamMemberPositions { TeamMemberId = teamMember.Id, PositionsId = newPosition.Id });
                    }
                }

                await _repository.SaveChangesAsync();
                return Result.Ok(_mapper.Map<TeamMemberDTO>(teamMember));
            }
            catch (Exception ex)
            {
                string errorMessage = _stringLocalizerFailed["FailedToCreateTeam"].Value;
                _logger.LogError(request, ex.Message);
                return Result.Fail(errorMessage);
            }
        }
    }
}