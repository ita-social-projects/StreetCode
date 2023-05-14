using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Team;
using Streetcode.DAL.Entities.Team;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Team.Update
{
    public class UpdateTeamHandler : IRequestHandler<UpdateTeamQuery, Result<TeamMemberDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;

        public UpdateTeamHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
        }

        public async Task<Result<TeamMemberDTO>> Handle(UpdateTeamQuery request, CancellationToken cancellationToken)
        {
            var team = _mapper.Map<TeamMember>(request.TeamMember);

            try
            {
                var links = await _repositoryWrapper.TeamLinkRepository
                   .GetAllAsync(predicate: l => l.TeamMemberId == team.Id);

                var newLinkIds = team.TeamMemberLinks.Select(l => l.Id).ToList();

                foreach (var link in links)
                {
                    if (!newLinkIds.Contains(link.Id))
                    {
                        _repositoryWrapper.TeamLinkRepository.Delete(link);
                    }
                }

                team.Positions.Clear();
                _repositoryWrapper.TeamRepository.Update(team);
                _repositoryWrapper.SaveChanges();
                var newPositionsIds = request.TeamMember.Positions.Select(s => s.Id).ToList();
                var oldPositions = await _repositoryWrapper.TeamPositionRepository
                    .GetAllAsync(ps => ps.TeamMemberId == team.Id);

                foreach (var old in oldPositions!)
                {
                    if (!newPositionsIds.Contains(old.PositionsId))
                    {
                        _repositoryWrapper.TeamPositionRepository.Delete(old);
                    }
                }

                foreach (var newCodeId in newPositionsIds!)
                {
                    if (oldPositions.FirstOrDefault(x => x.PositionsId == newCodeId) == null)
                    {
                        _repositoryWrapper.TeamPositionRepository.Create(
                            new TeamMemberPositions() { TeamMemberId = team.Id, PositionsId = newCodeId });
                    }
                }

                _repositoryWrapper.SaveChanges();
                var dbo = _mapper.Map<TeamMemberDTO>(team);
                dbo.Positions = request.TeamMember.Positions;
                return Result.Ok(dbo);
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }
        }
    }
}
