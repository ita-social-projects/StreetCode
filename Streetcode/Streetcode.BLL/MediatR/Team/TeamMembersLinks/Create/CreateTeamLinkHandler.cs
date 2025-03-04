using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Team.TeamMembersLinks.Create
{
    public class CreateTeamLinkHandler : IRequestHandler<CreateTeamLinkQuery, Result<TeamMemberLinkDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repository;
        private readonly ILoggerService _logger;
        private readonly IStringLocalizer<CannotConvertNullSharedResource> _stringLocalizerCannotConvert;
        private readonly IStringLocalizer<CannotCreateSharedResource> _stringLocalizerCannotCreate;
        private readonly IStringLocalizer<FailedToCreateSharedResource> _stringLocalizerFailedToCreate;

        public CreateTeamLinkHandler(
            IMapper mapper,
            IRepositoryWrapper repository,
            ILoggerService logger,
            IStringLocalizer<CannotCreateSharedResource> stringLocalizerCannotCreate,
            IStringLocalizer<FailedToCreateSharedResource> stringLocalizerFailedToCreate,
            IStringLocalizer<CannotConvertNullSharedResource> stringLocalizerCannotConvert)
        {
            _mapper = mapper;
            _repository = repository;
            _stringLocalizerCannotCreate = stringLocalizerCannotCreate;
            _stringLocalizerFailedToCreate = stringLocalizerFailedToCreate;
            _stringLocalizerCannotConvert = stringLocalizerCannotConvert;
            _logger = logger;
        }

        public async Task<Result<TeamMemberLinkDTO>> Handle(CreateTeamLinkQuery request, CancellationToken cancellationToken)
        {
            var teamMemberLink = _mapper.Map<DAL.Entities.Team.TeamMemberLink>(request.teamMember);

            if (teamMemberLink is null)
            {
                string errorMsg = _stringLocalizerCannotConvert["CannotConvertNullToTeamLink"].Value;
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            var createdTeamLink = await _repository.TeamLinkRepository.CreateAsync(teamMemberLink);

            if (createdTeamLink is null)
            {
                string errorMsg = _stringLocalizerCannotCreate["CannotCreateTeamLink"].Value;
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            var resultIsSuccess = await _repository.SaveChangesAsync() > 0;

            if (!resultIsSuccess)
            {
                string errorMsg = _stringLocalizerFailedToCreate["FailedToCreateTeam"].Value;
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            var createdTeamLinkDTO = _mapper.Map<TeamMemberLinkDTO>(createdTeamLink);

            if(createdTeamLinkDTO != null)
            {
                return Result.Ok(createdTeamLinkDTO);
            }
            else
            {
                string errorMsg = _stringLocalizerFailedToCreate["FailedToMapCreatedTeamLink"].Value;
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }
        }
    }
}
