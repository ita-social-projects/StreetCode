using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.MediatR.Partners.GetById;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Team.GetById
{
    public class GetByIdTeamHandler : IRequestHandler<GetByIdTeamQuery, Result<TeamMemberDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;

        public GetByIdTeamHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _stringLocalizerCannotFind = stringLocalizerCannotFind;
        }

        public async Task<Result<TeamMemberDTO>> Handle(GetByIdTeamQuery request, CancellationToken cancellationToken)
        {
            var team = await _repositoryWrapper
                .TeamRepository
                .GetSingleOrDefaultAsync(
                    predicate: p => p.Id == request.Id,
                    include: x => x.Include(x => x.TeamMemberLinks)
                    .Include(x => x.Positions));

            if (team is null)
            {
                return Result.Fail(new Error(_stringLocalizerCannotFind["CannotFindAnyTeamWithCorrespondingId", request.Id].Value));
            }

            var teamDto = _mapper.Map<TeamMemberDTO>(team);
            return Result.Ok(teamDto);
        }
    }
}