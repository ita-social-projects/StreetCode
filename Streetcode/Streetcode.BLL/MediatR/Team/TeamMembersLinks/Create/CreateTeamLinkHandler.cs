using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.MediatR.Team.Create;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Team.TeamMembersLinks.Create
{
    public class CreateTeamLinkHandler : IRequestHandler<CreateTeamLinkQuery, Result<TeamMemberLinkDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repository;
        private readonly IStringLocalizer<CannotConvertNullSharedResource> _stringLocalizerCannotConvert;
        private readonly IStringLocalizer<CannotCreateSharedResource> _stringLocalizerCannotCreate;
        private readonly IStringLocalizer<FailedToCreateSharedResource> _stringLocalizerFailedToCreate;

        public CreateTeamLinkHandler(
            IMapper mapper,
            IRepositoryWrapper repository,
            IStringLocalizer<CannotCreateSharedResource> stringLocalizerCannotCreate,
            IStringLocalizer<FailedToCreateSharedResource> stringLocalizerFailedToCreate,
            IStringLocalizer<CannotConvertNullSharedResource> stringLocalizerCannotConvert)
        {
            _mapper = mapper;
            _repository = repository;
            _stringLocalizerCannotCreate = stringLocalizerCannotCreate;
            _stringLocalizerFailedToCreate = stringLocalizerFailedToCreate;
            _stringLocalizerCannotConvert = stringLocalizerCannotConvert;
        }

        public async Task<Result<TeamMemberLinkDTO>> Handle(CreateTeamLinkQuery request, CancellationToken cancellationToken)
        {
            var teamMemberLink = _mapper.Map<DAL.Entities.Team.TeamMemberLink>(request.teamMember);

            if (teamMemberLink is null)
            {
                return Result.Fail(new Error(_stringLocalizerCannotConvert["CannotConvertNullToTeamLink"].Value));
            }

            var createdTeamLink = _repository.TeamLinkRepository.Create(teamMemberLink);

            if (createdTeamLink is null)
            {
                return Result.Fail(new Error(_stringLocalizerCannotCreate["CannotCreateTeamLink"].Value));
            }

            var resultIsSuccess = await _repository.SaveChangesAsync() > 0;

            if (!resultIsSuccess)
            {
                return Result.Fail(new Error(_stringLocalizerFailedToCreate["FailedToCreateTeam"].Value));
            }

            var createdTeamLinkDTO = _mapper.Map<TeamMemberLinkDTO>(createdTeamLink);

            return createdTeamLinkDTO != null ? Result.Ok(createdTeamLinkDTO) : Result.Fail(new Error(_stringLocalizerFailedToCreate["FailedToMapCreatedTeamLink"].Value));
        }
    }
}
