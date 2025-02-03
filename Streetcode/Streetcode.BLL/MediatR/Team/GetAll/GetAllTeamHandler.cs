using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.News;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Entities.Team;
using Streetcode.DAL.Helpers;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Team.GetAll
{
    public class GetAllTeamHandler : IRequestHandler<GetAllTeamQuery, Result<GetAllTeamDto>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;
        private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;

        public GetAllTeamHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger, IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
            _stringLocalizerCannotFind = stringLocalizerCannotFind;
        }

        public Task<Result<GetAllTeamDto>> Handle(GetAllTeamQuery request, CancellationToken cancellationToken)
        {
            PaginationResponse<TeamMember> paginationResponse = _repositoryWrapper
                .TeamRepository
                .GetAllPaginated(
                    request.page,
                    request.pageSize,
                    include: x => x.Include(x => x.Positions).Include(x => x.TeamMemberLinks!));

            if (paginationResponse is null)
            {
                string errorMsg = _stringLocalizerCannotFind["CannotFindAnyTeam"].Value;
                _logger.LogError(request, errorMsg);
                return Task.FromResult(Result.Fail<GetAllTeamDto>(new Error(errorMsg)));
            }

            GetAllTeamDto getAllTeamDTO = new GetAllTeamDto()
            {
                TotalAmount = paginationResponse.TotalItems,
                TeamMembers = _mapper.Map<IEnumerable<TeamMemberDto>>(paginationResponse.Entities),
            };

            return Task.FromResult(Result.Ok(getAllTeamDTO));
        }
    }
}