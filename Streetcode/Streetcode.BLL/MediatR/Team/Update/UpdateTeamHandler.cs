using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Entities.Team;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Team.Update
{
    public class UpdateTeamHandler : IRequestHandler<UpdateTeamQuery, Result<UpdateTeamMemberDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;

        public UpdateTeamHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<UpdateTeamMemberDTO>> Handle(UpdateTeamQuery request, CancellationToken cancellationToken)
        {
            var team = _mapper.Map<TeamMember>(request.TeamMember);

            try
            {
                var links = await _repositoryWrapper.TeamLinkRepository
                   .GetAllAsync(predicate: l => l.TeamMemberId == team.Id);

                var newLinkIds = team.TeamMemberLinks?.Select(l => l.Id).ToList() ?? new();

                foreach (var link in links)
                {
                    if (!newLinkIds.Contains(link.Id))
                    {
                        _repositoryWrapper.TeamLinkRepository.Delete(link);
                    }
                }

                team.Positions?.Clear();
                _repositoryWrapper.TeamRepository.Update(team);
                await _repositoryWrapper.SaveChangesAsync();

                var newPositions = request.TeamMember.Positions!.ToList();
                var newPositionsIds = newPositions.Select(s => s.Id).ToList();

                var oldPositions = await _repositoryWrapper.TeamPositionRepository
                    .GetAllAsync(ps => ps.TeamMemberId == team.Id);

                foreach (var old in oldPositions!)
                {
                    if (!newPositionsIds.Contains(old.PositionsId))
                    {
                        _repositoryWrapper.TeamPositionRepository.Delete(old);
                    }
                }

                foreach (var newPosition in newPositions)
                {
                    if (newPosition.Id < 0)
                    {
                        Positions position = new() { Id = 0, Position = newPosition.Position, TeamMembers = null };
                        var tpm = await _repositoryWrapper.PositionRepository.CreateAsync(position);

                        await _repositoryWrapper.SaveChangesAsync();

                        await _repositoryWrapper.TeamPositionRepository.CreateAsync(
                            new TeamMemberPositions { TeamMemberId = team.Id, PositionsId = tpm.Id });
                    }
                    else if (oldPositions.FirstOrDefault(x => x.PositionsId == newPosition.Id) == null)
                    {
                        await _repositoryWrapper.TeamPositionRepository.CreateAsync(
                            new TeamMemberPositions { TeamMemberId = team.Id, PositionsId = newPosition.Id });
                    }
                }

                await _repositoryWrapper.SaveChangesAsync();
                var dbo = _mapper.Map<UpdateTeamMemberDTO>(team);
                dbo.Positions = newPositions;
                return Result.Ok(dbo);
            }
            catch (Exception ex)
            {
                _logger.LogError(request, ex.Message);
                return Result.Fail(ex.Message);
            }
        }
    }
}