using FluentResults;
using MediatR;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.RelatedFigure.Delete;

public class DeleteRelatedFigureHandler : IRequestHandler<DeleteRelatedFigureCommand, Result<Unit>>
{
    private readonly IRepositoryWrapper _repositoryWrapper;

    public DeleteRelatedFigureHandler(IRepositoryWrapper repositoryWrapper)
    {
        _repositoryWrapper = repositoryWrapper;
    }

    public async Task<Result<Unit>> Handle(DeleteRelatedFigureCommand request, CancellationToken cancellationToken)
    {
        var relation = await _repositoryWrapper.RelatedFigureRepository
                                .GetFirstOrDefaultAsync(rel =>
                                rel.ObserverId == request.ObserverId &&
                                rel.TargetId == request.TargetId);

        if (relation is null)
        {
            return Result.Fail(new Error($"Cannot find a relation between streetcodes with corresponding ids: {request.ObserverId} & {request.TargetId}"));
        }

        _repositoryWrapper.RelatedFigureRepository.Delete(relation);

        var resultIsSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;
        return resultIsSuccess ? Result.Ok(Unit.Value) : Result.Fail(new Error("Failed to delete a relation."));
    }
}
