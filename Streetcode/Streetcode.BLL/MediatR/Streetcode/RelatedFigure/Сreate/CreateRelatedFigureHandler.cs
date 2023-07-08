using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using NLog.Targets;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.RelatedFigure.Create;

public class CreateRelatedFigureHandler : IRequestHandler<CreateRelatedFigureCommand, Result<Unit>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IStringLocalizer<NoSharedResource> _stringLocalizerNo;
    private readonly IStringLocalizer<FailedToCreateSharedResource> _stringLocalizerFailed;
    public CreateRelatedFigureHandler(
        IRepositoryWrapper repositoryWrapper,
        IMapper mapper,
        IStringLocalizer<NoSharedResource> stringLocalizerNo,
        IStringLocalizer<FailedToCreateSharedResource> stringLocalizerFailed)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _stringLocalizerFailed = stringLocalizerFailed;
        _stringLocalizerNo = stringLocalizerNo;
    }

    public async Task<Result<Unit>> Handle(CreateRelatedFigureCommand request, CancellationToken cancellationToken)
    {
        var observerEntity = await _repositoryWrapper.StreetcodeRepository.GetFirstOrDefaultAsync(rel => rel.Id == request.ObserverId);
        var targetEntity = await _repositoryWrapper.StreetcodeRepository.GetFirstOrDefaultAsync(rel => rel.Id == request.TargetId);

        if (observerEntity is null)
        {
            return Result.Fail(new Error(_stringLocalizerNo["NoExistingStreetcodeWithId", request.ObserverId]));
        }

        if (targetEntity is null)
        {
            return Result.Fail(new Error(_stringLocalizerNo["NoExistingStreetcodeWithId", request.TargetId]));
        }

        var relation = new DAL.Entities.Streetcode.RelatedFigure
        {
            ObserverId = observerEntity.Id,
            TargetId = targetEntity.Id,
        };

        _repositoryWrapper.RelatedFigureRepository.Create(relation);

        var resultIsSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;
        return resultIsSuccess ? Result.Ok(Unit.Value) : Result.Fail(new Error(_stringLocalizerFailed["FailedToCreateRelation"].Value));
    }
}
