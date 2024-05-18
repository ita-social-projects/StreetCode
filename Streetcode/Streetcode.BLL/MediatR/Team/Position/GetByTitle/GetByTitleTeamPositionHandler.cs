using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Team.Position.GetByTitle;

public class GetByTitleTeamPositionHandler : IRequestHandler<GetByTitleTeamPositionQuery, Result<PositionDTO>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repository;
    private readonly ILoggerService _loggerService;

    public GetByTitleTeamPositionHandler(IMapper mapper, IRepositoryWrapper repository, ILoggerService loggerService)
    {
        _mapper = mapper;
        _repository = repository;
        _loggerService = loggerService;
    }

    public async Task<Result<PositionDTO>> Handle(GetByTitleTeamPositionQuery request, CancellationToken cancellationToken)
    {
        var position = _repository.PositionRepository.GetFirstOrDefaultAsync(j => j.Position == request.position);

        if (position is null)
        {
            string exceptionMessege = $"No position found by title - {request.position}";
            _loggerService.LogError(request, exceptionMessege);
            return Result.Fail(exceptionMessege);
        }

        try
        {
            var positionDto = _mapper.Map<PositionDTO>(position);
            return Result.Ok(positionDto);
        }
        catch (Exception ex)
        {
            _loggerService.LogError(request, ex.Message);
            return Result.Fail(ex.Message);
        }
    }
}
