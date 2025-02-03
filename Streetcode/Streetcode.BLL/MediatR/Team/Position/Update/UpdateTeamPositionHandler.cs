using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Team;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Team.Position.Update;

public class UpdateTeamPositionHandler : IRequestHandler<UpdateTeamPositionCommand, Result<PositionDto>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repository;
    private readonly ILoggerService _logger;
    private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizer;

    public UpdateTeamPositionHandler(IRepositoryWrapper repository, IMapper mapper, ILoggerService logger, IStringLocalizer<CannotFindSharedResource> localizer)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
        _stringLocalizer = localizer;
    }

    public async Task<Result<PositionDto>> Handle(UpdateTeamPositionCommand request, CancellationToken cancellationToken)
    {
        var position = await _repository.PositionRepository.GetFirstOrDefaultAsync(x => x.Id == request.positionDto.Id);

        if (position is null)
        {
            string exMessage = _stringLocalizer["CannotFindPositionWithCorrespondingId", request.positionDto.Id];
            _logger.LogError(request, exMessage);
            return Result.Fail(exMessage);
        }

        try
        {
            var positionToUpdate = _mapper.Map<Positions>(request.positionDto);
            _repository.PositionRepository.Update(positionToUpdate);
            await _repository.SaveChangesAsync();
            var positionDto = _mapper.Map<PositionDto>(positionToUpdate);
            return Result.Ok(positionDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(request, ex.Message);
            return Result.Fail(ex.Message);
        }
    }
}
