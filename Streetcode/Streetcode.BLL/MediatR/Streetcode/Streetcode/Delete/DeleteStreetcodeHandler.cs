using FluentResults;
using MediatR;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.Delete;

public class DeleteStreetcodeHandler : IRequestHandler<DeleteStreetcodeCommand, Result<Unit>>
{
    private readonly IRepositoryWrapper _repositoryWrapper;

    public DeleteStreetcodeHandler(IRepositoryWrapper repositoryWrapper)
    {
        _repositoryWrapper = repositoryWrapper;
    }

    public async Task<Result<Unit>> Handle(DeleteStreetcodeCommand request, CancellationToken cancellationToken)
    {
        var streetcodesRelated = await _repositoryWrapper.RelatedFigureRepository
            .GetAllAsync(sc => sc.ObserverId == request.Id || sc.TargetId == request.Id);

        foreach (var streetcodeRelated in streetcodesRelated)
        {
            _repositoryWrapper.RelatedFigureRepository.Delete(streetcodeRelated);
        }

        var streetcode = await _repositoryWrapper.StreetcodeRepository.GetFirstOrDefaultAsync(f => f.Id == request.Id);

        if (streetcode is null)
        {
            return Result.Fail(new Error($"Cannot find a streetcode with corresponding categoryId: {request.Id}"));
        }

        _repositoryWrapper.StreetcodeRepository.Delete(streetcode);

        var resultIsSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;
        return resultIsSuccess ? Result.Ok(Unit.Value) : Result.Fail(new Error("Failed to delete a streetcode"));
    }
}