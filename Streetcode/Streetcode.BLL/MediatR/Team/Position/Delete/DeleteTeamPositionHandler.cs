using FluentResults;
using MediatR;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Team.Position.Delete;

public class DeleteTeamPositionHandler : IRequestHandler<DeleteTeamPositionCommand, Result<int>>
{
    private readonly IRepositoryWrapper _repository;
    private readonly ILoggerService _logger;

    public DeleteTeamPositionHandler(IRepositoryWrapper repository, ILoggerService logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<int>> Handle(DeleteTeamPositionCommand request, CancellationToken cancellationToken)
    {
        var positionToDelete =
            await _repository.PositionRepository.GetFirstOrDefaultAsync(x => x.Id == request.positionId);

        if (positionToDelete is null)
        {
            string exMessage = $"No position found by entered Id - {request.positionId}";
            _logger.LogError(request, exMessage);
            return Result.Fail(exMessage);
        }

        try
        {
            _repository.PositionRepository.Delete(positionToDelete);
            await _repository.SaveChangesAsync();
            return Result.Ok(request.positionId);
        }
        catch (Exception ex)
        {
            _logger.LogError(request, ex.Message);
            return Result.Fail(ex.Message);
        }
    }
}
