using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.RelatedFigure.Delete;

public class DeleteRelatedFigureHandler : IRequestHandler<DeleteRelatedFigureCommand, Result<Unit>>
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService _logger;
    private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;
    private readonly IStringLocalizer<FailedToDeleteSharedResource> _stringLocalizerFailedToDelete;

    public DeleteRelatedFigureHandler(
        IRepositoryWrapper repositoryWrapper,
        ILoggerService logger,
        IStringLocalizer<FailedToDeleteSharedResource> stringLocalizerFailedToDelete,
        IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind)
    {
        _repositoryWrapper = repositoryWrapper;
        _logger = logger;
        _stringLocalizerFailedToDelete = stringLocalizerFailedToDelete;
        _stringLocalizerCannotFind = stringLocalizerCannotFind;
    }

    public async Task<Result<Unit>> Handle(DeleteRelatedFigureCommand request, CancellationToken cancellationToken)
    {
        var relation = await _repositoryWrapper.RelatedFigureRepository
            .GetFirstOrDefaultAsync(rel =>
                rel.ObserverId == request.ObserverId &&
                rel.TargetId == request.TargetId);

        if (relation is null)
        {
            var errorMsg = _stringLocalizerCannotFind["CannotFindRelationBetweenStreetcodesWithCorrespondingIds", request.ObserverId, request.TargetId].Value;
            _logger.LogError(request, errorMsg);
            return Result.Fail(new Error(errorMsg));
        }

        _repositoryWrapper.RelatedFigureRepository.Delete(relation);

        var resultIsSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;
        if(resultIsSuccess)
        {
            return Result.Ok(Unit.Value);
        }

        var finalErrorMsg = _stringLocalizerFailedToDelete["FailedToDeleteRelation"].Value;
        _logger.LogError(request, finalErrorMsg);
        return Result.Fail(new Error(finalErrorMsg));
    }
}
