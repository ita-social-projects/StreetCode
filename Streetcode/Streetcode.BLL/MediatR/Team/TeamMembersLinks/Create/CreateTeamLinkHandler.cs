using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.MediatR.Team.Create;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Team.TeamMembersLinks.Create
{
    public class CreateTeamLinkHandler : IRequestHandler<CreateTeamLinkQuery, Result<TeamMemberLinkDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repository;

        public CreateTeamLinkHandler(IMapper mapper, IRepositoryWrapper repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<Result<TeamMemberLinkDTO>> Handle(CreateTeamLinkQuery request, CancellationToken cancellationToken)
        {
            var teamMemberLink = _mapper.Map<DAL.Entities.Team.TeamMemberLink>(request.teamMember);

            if (teamMemberLink is null)
            {
                return Result.Fail(new Error("Cannot convert null to team link"));
            }

            var createdTeamLink = _repository.TeamLinkRepository.Create(teamMemberLink);

            if (createdTeamLink is null)
            {
                return Result.Fail(new Error("Cannot create team link"));
            }

            var resultIsSuccess = await _repository.SaveChangesAsync() > 0;

            if (!resultIsSuccess)
            {
                return Result.Fail(new Error("Failed to create a team"));
            }

            var createdTeamLinkDTO = _mapper.Map<TeamMemberLinkDTO>(createdTeamLink);

            return createdTeamLinkDTO != null ? Result.Ok(createdTeamLinkDTO) : Result.Fail(new Error("Failed to map created team link"));
        }
    }
}
