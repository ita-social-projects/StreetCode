using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Team;
using Streetcode.DAL.Enums;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Team.Create
{
    public class CreateTeamHandler : IRequestHandler<CreateTeamQuery, Result<TeamMemberDto>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repository;
        private readonly ILoggerService _logger;
        private readonly IStringLocalizer<CannotConvertNullSharedResource> _stringLocalizerCannot;

        public CreateTeamHandler(
            IMapper mapper,
            IRepositoryWrapper repository,
            ILoggerService logger,
            IStringLocalizer<CannotConvertNullSharedResource> stringLocalizerCannot)
        {
            _mapper = mapper;
            _repository = repository;
            _logger = logger;
            _stringLocalizerCannot = stringLocalizerCannot;
        }

        public async Task<Result<TeamMemberDto>> Handle(CreateTeamQuery request, CancellationToken cancellationToken)
        {
            var teamMember = _mapper.Map<TeamMember>(request.teamMember);
            try
            {
                teamMember.Positions!.Clear();

                var newLogoTypes = request.teamMember.TeamMemberLinks.Select(links => links.LogoType).ToList();

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
                var resulted = _mapper.Map<TeamMemberDto>(teamMember);
                return Result.Ok(_mapper.Map<TeamMemberDto>(teamMember));
            }
            catch (Exception ex)
            {
                _logger.LogError(request, ex.Message);
                return Result.Fail(ex.Message);
            }
        }
    }
}