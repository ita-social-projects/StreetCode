using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.AdditionalContent.Subtitles;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Team.TeamMembersLinks.GetAll
{
    public class GetAllTeamLinkHandler : IRequestHandler<GetAllTeamLinkQuery, Result<IEnumerable<TeamMemberLinkDTO>>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;
        private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;

        public GetAllTeamLinkHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger, IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
            _stringLocalizerCannotFind = stringLocalizerCannotFind;
        }

        public async Task<Result<IEnumerable<TeamMemberLinkDTO>>> Handle(GetAllTeamLinkQuery request, CancellationToken cancellationToken)
        {
            var teamLinks = await _repositoryWrapper
                .TeamLinkRepository
                .GetAllAsync();

            if (teamLinks is null)
            {
                const string errorMsg = _stringLocalizerCannotFind["CannotFindAnyTeamLinks"].Value;
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            return Result.Ok(_mapper.Map<IEnumerable<TeamMemberLinkDTO>>(teamLinks));
        }
    }
}
