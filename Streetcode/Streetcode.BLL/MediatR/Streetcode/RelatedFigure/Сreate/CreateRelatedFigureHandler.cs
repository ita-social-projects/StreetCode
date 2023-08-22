using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using NLog.Targets;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.RelatedFigure.Create;

public class CreateRelatedFigureHandler : IRequestHandler<CreateRelatedFigureCommand, Result<Unit>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService _logger;
    private readonly IStringLocalizer<NoSharedResource> _stringLocalizerNo;
    private readonly IStringLocalizer<FailedToCreateSharedResource> _stringLocalizerFailed;
    public CreateRelatedFigureHandler(
        IRepositoryWrapper repositoryWrapper,
        IMapper mapper,
        ILoggerService logger,
        IStringLocalizer<NoSharedResource> stringLocalizerNo,
        IStringLocalizer<FailedToCreateSharedResource> stringLocalizerFailed)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
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
            string errorMsg = _stringLocalizerNo["NoExistingStreetcodeWithId", request.ObserverId].Value;
            _logger.LogError(request, errorMsg);
            return Result.Fail(new Error(errorMsg));
        }

        if (targetEntity is null)
        {
            string errorMsg = _stringLocalizerNo["NoExistingStreetcodeWithId", request.TargetId].Value;
            _logger.LogError(request, errorMsg);
            return Result.Fail(new Error(errorMsg));
        }

        var relation = new DAL.Entities.Streetcode.RelatedFigure
        {
            ObserverId = observerEntity.Id,
            TargetId = targetEntity.Id,
        };

        _repositoryWrapper.RelatedFigureRepository.Create(relation);

        var resultIsSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;
        if(resultIsSuccess)
        {
            return Result.Ok(Unit.Value);
        }
        else
        {
            string errorMsg = _stringLocalizerFailed["FailedToCreateRelation"].Value;
            _logger.LogError(request, errorMsg);
            return Result.Fail(new Error(errorMsg));
        }
    }
}
