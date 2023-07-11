using AutoMapper;
using FluentResults;
using MediatR;
using NLog.Targets;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.RelatedFigure.Create;

public class CreateRelatedFigureHandler : IRequestHandler<CreateRelatedFigureCommand, Result<Unit>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService _logger;

    public CreateRelatedFigureHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<Unit>> Handle(CreateRelatedFigureCommand request, CancellationToken cancellationToken)
    {
        var observerEntity = await _repositoryWrapper.StreetcodeRepository.GetFirstOrDefaultAsync(rel => rel.Id == request.ObserverId);
        var targetEntity = await _repositoryWrapper.StreetcodeRepository.GetFirstOrDefaultAsync(rel => rel.Id == request.TargetId);

        if (observerEntity is null)
        {
            string errorMsg = $"No existing streetcode with id: {request.ObserverId}";
            _logger.LogError(request, errorMsg);
            return Result.Fail(new Error(errorMsg));
        }

        if (targetEntity is null)
        {
            string errorMsg = $"No existing streetcode with id: {request.TargetId}";
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
            string errorMsg = "Failed to create a relation.";
            _logger.LogError(request, errorMsg);
            return Result.Fail(new Error(errorMsg));
        }
    }
}
