using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Entity = Streetcode.DAL.Entities.Streetcode.RelatedFigure;

namespace Streetcode.BLL.MediatR.Streetcode.RelatedFigure.Create;

public class CreateRelatedFigureHandler : IRequestHandler<CreateRelatedFigureCommand, Result<Unit>>
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService _logger;
    private readonly IStringLocalizer<NoSharedResource> _stringLocalizerNo;
    private readonly IStringLocalizer<FailedToCreateSharedResource> _stringLocalizerFailed;

    public CreateRelatedFigureHandler(
        IRepositoryWrapper repositoryWrapper,
        ILoggerService logger,
        IStringLocalizer<NoSharedResource> stringLocalizerNo,
        IStringLocalizer<FailedToCreateSharedResource> stringLocalizerFailed)
    {
        _repositoryWrapper = repositoryWrapper;
        _logger = logger;
        _stringLocalizerFailed = stringLocalizerFailed;
        _stringLocalizerNo = stringLocalizerNo;
    }

    public async Task<Result<Unit>> Handle(CreateRelatedFigureCommand request, CancellationToken cancellationToken)
    {
        var observerEntity = await _repositoryWrapper.StreetcodeRepository.GetFirstOrDefaultAsync(rel => rel.Id == request.ObserverId);
        var targetEntity = await _repositoryWrapper.StreetcodeRepository.GetFirstOrDefaultAsync(rel => rel.Id == request.TargetId);

        if (observerEntity is null)
        {
            var errorMsg = _stringLocalizerNo["NoExistingStreetcodeWithId", request.ObserverId].Value;
            _logger.LogError(request, errorMsg);
            return Result.Fail(new Error(errorMsg));
        }

        if (targetEntity is null)
        {
            var errorMsg = _stringLocalizerNo["NoExistingStreetcodeWithId", request.TargetId].Value;
            _logger.LogError(request, errorMsg);
            return Result.Fail(new Error(errorMsg));
        }

        var existingRelation = await _repositoryWrapper.RelatedFigureRepository.GetFirstOrDefaultAsync(rel =>
            (rel.ObserverId == request.ObserverId && rel.TargetId == request.TargetId) ||
            (rel.ObserverId == request.TargetId && rel.TargetId == request.ObserverId));

        if (existingRelation is not null)
        {
            var errorMsg = _stringLocalizerFailed["TheStreetcodesAreAlreadyLinked"].Value;
            _logger.LogError(request, errorMsg);
            return Result.Fail(new Error(errorMsg));
        }

        var relation = new Entity
        {
            ObserverId = observerEntity.Id,
            TargetId = targetEntity.Id,
        };

        await _repositoryWrapper.RelatedFigureRepository.CreateAsync(relation);

        var resultIsSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;
        if(resultIsSuccess)
        {
            return Result.Ok(Unit.Value);
        }

        var finalErrorMsg = _stringLocalizerFailed["FailedToCreateRelation"].Value;
        _logger.LogError(request, finalErrorMsg);
        return Result.Fail(new Error(finalErrorMsg));
    }
}
