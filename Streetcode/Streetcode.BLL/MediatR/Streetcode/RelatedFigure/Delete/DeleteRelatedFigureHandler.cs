using FluentResults;
using MediatR;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.RelatedFigure.Delete;

public class DeleteRelatedFigureHandler : IRequestHandler<DeleteRelatedFigureCommand, Result<Unit>>
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService? _logger;

    public DeleteRelatedFigureHandler(IRepositoryWrapper repositoryWrapper, ILoggerService? logger = null)
    {
        _repositoryWrapper = repositoryWrapper;
        _logger = logger;
    }

    public async Task<Result<Unit>> Handle(DeleteRelatedFigureCommand request, CancellationToken cancellationToken)
    {
        var relation = await _repositoryWrapper.RelatedFigureRepository
                                .GetFirstOrDefaultAsync(rel =>
                                rel.ObserverId == request.ObserverId &&
                                rel.TargetId == request.TargetId);

        if (relation is null)
        {
            string errorMsg = $"Cannot find a relation between streetcodes with corresponding ids: {request.ObserverId} & {request.TargetId}";
            _logger?.LogError("DeleteRelatedFigureCommand handled with an error");
            _logger?.LogError(errorMsg);
            return Result.Fail(new Error(errorMsg));
        }

        _repositoryWrapper.RelatedFigureRepository.Delete(relation);

        var resultIsSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;
        if(resultIsSuccess)
        {
            _logger?.LogInformation($"DeleteRelatedFigureCommand handled successfully");
            return Result.Ok(Unit.Value);
        }
        else
        {
            const string errorMsg = "Failed to delete a relation.";
            _logger?.LogError("DeleteRelatedFigureCommand handled with an error");
            _logger?.LogError(errorMsg);
            return Result.Fail(new Error(errorMsg));
        }
    }
}
