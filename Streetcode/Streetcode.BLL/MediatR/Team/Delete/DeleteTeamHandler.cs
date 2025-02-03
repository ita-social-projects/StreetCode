using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Team.Delete
{
    public class DeleteTeamHandler : IRequestHandler<DeleteTeamQuery, Result<TeamMemberDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;
        private readonly IStringLocalizer<NoSharedResource> _stringLocalizerNo;

        public DeleteTeamHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger, IStringLocalizer<NoSharedResource> stringLocalizerNo)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
            _stringLocalizerNo = stringLocalizerNo;
        }

        public async Task<Result<TeamMemberDTO>> Handle(DeleteTeamQuery request, CancellationToken cancellationToken)
        {
            var team = await _repositoryWrapper.TeamRepository.GetFirstOrDefaultAsync(p => p.Id == request.id);
            if (team == null)
            {
                string errorMsg = _stringLocalizerNo["NoTeamWithSuchId"].Value;
                _logger.LogError(request, errorMsg);
                return Result.Fail(errorMsg);
            }
            else
            {
                _repositoryWrapper.TeamRepository.Delete(team);
                try
                {
                    await _repositoryWrapper.SaveChangesAsync();
                    return Result.Ok(_mapper.Map<TeamMemberDTO>(team));
                }
                catch (Exception ex)
                {
                    _logger.LogError(request, ex.Message);
                    return Result.Fail(ex.Message);
                }
            }
        }
    }
}