using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.DTO.Team;
using Streetcode.DAL.Entities.Partners;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.DAL.Repositories.Realizations.Base;

namespace Streetcode.BLL.MediatR.Team.Create
{
    public class CreateTeamHandler : IRequestHandler<CreateTeamQuery, Result<TeamMemberDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repository;

        public CreateTeamHandler(IMapper mapper, IRepositoryWrapper repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<Result<TeamMemberDTO>> Handle(CreateTeamQuery request, CancellationToken cancellationToken)
        {
            var teamMember = _mapper.Map<DAL.Entities.Team.TeamMember>(request.teamMember);
            try
            {
                teamMember.Positions.Clear();
                teamMember.TeamMemberLinks.Clear();
                teamMember = await _repository.TeamRepository.CreateAsync(teamMember);
                _repository.SaveChanges();
                var positionsId = request.teamMember.Positions.Select(s => s.Id).ToList();
                teamMember.Positions.AddRange(await _repository.PositionRepository.GetAllAsync(s => positionsId.Contains(s.Id)));
                var linksId = request.teamMember.TeamMemberLinks.Select(l => l.Id).ToList();
                teamMember.TeamMemberLinks.AddRange(await _repository.TeamLinkRepository.GetAllAsync(l => linksId.Contains(l.Id)));
                _repository.SaveChanges();
                return Result.Ok(_mapper.Map<TeamMemberDTO>(teamMember));
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }
        }
    }
}
