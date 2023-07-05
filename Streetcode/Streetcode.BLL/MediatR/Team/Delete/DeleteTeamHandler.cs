using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Partners.Delete;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Team.Delete
{
    public class DeleteTeamHandler : IRequestHandler<DeleteTeamQuery, Result<TeamMemberDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;

        public DeleteTeamHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<TeamMemberDTO>> Handle(DeleteTeamQuery request, CancellationToken cancellationToken)
        {
            var team = await _repositoryWrapper.TeamRepository.GetFirstOrDefaultAsync(p => p.Id == request.id);
            if (team == null)
            {
                const string errorMsg = "No team with such id";
                _logger.LogError($"DeleteTeamQuery handled with an error. {errorMsg}");
                return Result.Fail(errorMsg);
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
                    _logger.LogError($"DeleteTeamQuery handled with an error. {ex.Message}");
                    return Result.Fail(ex.Message);
                }
            }
        }
    }
}