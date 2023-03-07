using AutoMapper;
using FluentResults;
using MediatR;
using NLog.Targets;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.RelatedFigure.Create;

public class CreateRelatedFigureHandler : IRequestHandler<CreateRelatedFigureCommand, Result<Unit>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public CreateRelatedFigureHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
    }

    public async Task<Result<Unit>> Handle(CreateRelatedFigureCommand request, CancellationToken cancellationToken)
    {
        var entity1 = await _repositoryWrapper.StreetcodeRepository.GetFirstOrDefaultAsync(rel => rel.Id == request.Id1);
        var entity2 = await _repositoryWrapper.StreetcodeRepository.GetFirstOrDefaultAsync(rel => rel.Id == request.Id2);

        if (entity1 is null)
        {
            return Result.Fail(new Error($"No existing streetcode with id: {request.Id1}"));
        }

        if (entity2 is null)
        {
            return Result.Fail(new Error($"No existing streetcode with id: {request.Id2}"));
        }

        var relation = new DAL.Entities.Streetcode.RelatedFigure
        {
            ObserverId = entity1.Id,
            TargetId = entity2.Id,
        };

        _repositoryWrapper.RelatedFigureRepository.Create(relation);

        var resultIsSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;
        return resultIsSuccess ? Result.Ok(Unit.Value) : Result.Fail(new Error("Failed to create a relation."));
    }
}