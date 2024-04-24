using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Org.BouncyCastle.Asn1.Cmp;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Team;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Team.Create
{
    public class CreateTeamHandler : IRequestHandler<CreateTeamQuery, Result<TeamMemberDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repository;
        private readonly ILoggerService _logger;

        public CreateTeamHandler(
            IMapper mapper,
            IRepositoryWrapper repository,
            ILoggerService logger)
        {
            _mapper = mapper;
            _repository = repository;
            _logger = logger;
        }

        public async Task<Result<TeamMemberDTO>> Handle(CreateTeamQuery request, CancellationToken cancellationToken)
        {
            var teamMember = _mapper.Map<TeamMember>(request.teamMember);
            if (teamMember.ImageId == 0)
            {
                string errormsg = "Invalid ImageId Value";
                _logger.LogError(request, errormsg);
                return Result.Fail(errormsg);
            }

            try
            {
                teamMember.Positions.Clear();

                teamMember = await _repository.TeamRepository.CreateAsync(teamMember);
                _repository.SaveChanges();

                var newPositions = request.teamMember.Positions.ToList();

                foreach (var newPosition in newPositions)
                {
                    if (newPosition.Id < 0)
                    {
                        Positions position = new Positions() { Id = 0, Position = newPosition.Position, TeamMembers = null };
                        var tpm = _repository.PositionRepository.Create(position);

                        _repository.SaveChanges();

                        _repository.TeamPositionRepository.Create(
                            new TeamMemberPositions { TeamMemberId = teamMember.Id, PositionsId = tpm.Id });
                    }
                    else
                    {
                        _repository.TeamPositionRepository.Create(
                            new TeamMemberPositions { TeamMemberId = teamMember.Id, PositionsId = newPosition.Id });
                    }
                }

                _repository.SaveChanges();
                var resulted = _mapper.Map<TeamMemberDTO>(teamMember);
                return Result.Ok(_mapper.Map<TeamMemberDTO>(teamMember));
            }
            catch (Exception ex)
            {
                _logger.LogError(request, ex.Message);
                return Result.Fail(ex.Message);
            }
        }
    }
}