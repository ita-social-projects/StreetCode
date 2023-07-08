using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.MediatR.Partners.Delete;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Team.Delete
{
    public class DeleteTeamHandler : IRequestHandler<DeleteTeamQuery, Result<TeamMemberDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IStringLocalizer<NoSharedResource> _stringLocalizerNo;

        public DeleteTeamHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, IStringLocalizer<NoSharedResource> stringLocalizerNo)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _stringLocalizerNo = stringLocalizerNo;
        }

        public async Task<Result<TeamMemberDTO>> Handle(DeleteTeamQuery request, CancellationToken cancellationToken)
        {
            var team = await _repositoryWrapper.TeamRepository.GetFirstOrDefaultAsync(p => p.Id == request.id);
            if (team == null)
            {
                return Result.Fail(_stringLocalizerNo["NoTeamWithSuchId"].Value);
            }
            else
            {
                _repositoryWrapper.TeamRepository.Delete(team);
                try
                {
                    _repositoryWrapper.SaveChanges();
                    return Result.Ok(_mapper.Map<TeamMemberDTO>(team));
                }
                catch (Exception ex)
                {
                    return Result.Fail(ex.Message);
                }
            }
        }
    }
}