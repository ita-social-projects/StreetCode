using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Team.Position.GetById;

public class GetByIdTeamPositionHandler : IRequestHandler<GetByIdTeamPositionQuery, Result<PositionDTO>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repository;
    private readonly ILoggerService _loggerService;

    public GetByIdTeamPositionHandler(IMapper mapper, IRepositoryWrapper repository, ILoggerService loggerService)
    {
        _mapper = mapper;
        _repository = repository;
        _loggerService = loggerService;
    }

    public async Task<Result<PositionDTO>> Handle(GetByIdTeamPositionQuery request, CancellationToken cancellationToken)
    {
        var position = await _repository.PositionRepository.GetFirstOrDefaultAsync(j => j.Id == request.positionId);

        if (position is null)
        {
            string exceptionMessege = $"No position found by entered Id - {request.positionId}";
            _loggerService.LogError(request, exceptionMessege);
            return Result.Fail(exceptionMessege);
        }

        try
        {
            var positionDTO = _mapper.Map<PositionDTO>(position);
            return Result.Ok(positionDTO);
        }
        catch (Exception ex)
        {
            _loggerService.LogError(request, ex.Message);
            return Result.Fail(ex.Message);
        }
    }
}
