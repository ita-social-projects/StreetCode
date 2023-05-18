using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.MediatR.Partners.Delete;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Team.Delete
{
    public class DeleteTeamHandler : IRequestHandler<DeleteTeamQuery, Result<TeamMemberDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;

        public DeleteTeamHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
        }

        public async Task<Result<TeamMemberDTO>> Handle(DeleteTeamQuery request, CancellationToken cancellationToken)
        {
            var team = await _repositoryWrapper.TeamRepository.GetFirstOrDefaultAsync(p => p.Id == request.id);
            if (team == null)
            {
                return Result.Fail("No team with such id");
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