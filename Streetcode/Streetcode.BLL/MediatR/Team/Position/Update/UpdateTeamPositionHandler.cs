using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Entities.Team;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Team.Position.Update;

public class UpdateTeamPositionHandler : IRequestHandler<UpdateTeamPositionCommand, Result<PositionDTO>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repository;
    private readonly ILoggerService _logger;

    public UpdateTeamPositionHandler(IRepositoryWrapper repository, IMapper mapper, ILoggerService logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<PositionDTO>> Handle(UpdateTeamPositionCommand request, CancellationToken cancellationToken)
    {
        var position = await _repository.PositionRepository.GetFirstOrDefaultAsync(x => x.Id == request.positionDto.Id);

        if (position is null)
        {
            string exMessage = $"No position found by entered Id - {request.positionDto.Id}";
            _logger.LogError(request, exMessage);
            return Result.Fail(exMessage);
        }

        var positionRepeat = await _repository.PositionRepository.GetFirstOrDefaultAsync(
            x =>
                x.Position == request.positionDto.Position);

        if (positionRepeat is not null)
        {
            string exMessage = $"There is already a position with title - {request.positionDto.Position}";
            _logger.LogError(request, exMessage);
            return Result.Fail(exMessage);
        }

        try
        {
            var positionToUpdate = _mapper.Map<Positions>(request.positionDto);
            _repository.PositionRepository.Update(positionToUpdate);
            await _repository.SaveChangesAsync();
            var positionDto = _mapper.Map<PositionDTO>(positionToUpdate);
            return Result.Ok(positionDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(request, ex.Message);
            return Result.Fail(ex.Message);
        }
    }
}
