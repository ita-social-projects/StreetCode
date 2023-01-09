using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.RelatedFigure.GetByStreetcodeId;

public class GetRelatedFiguresByStreetcodeIdHandler : IRequestHandler<GetRelatedFigureByStreetcodeIdQuery, Result<IEnumerable<RelatedFigureDTO>>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public GetRelatedFiguresByStreetcodeIdHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper)
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
    }

    public async Task<Result<IEnumerable<RelatedFigureDTO>>> Handle(GetRelatedFigureByStreetcodeIdQuery request, CancellationToken cancellationToken)
    {
        var observers = _repositoryWrapper.RelatedFigureRepository
            .FindAll(f => f.TargetId == request.StreetcodeId).Select(o => o.Observer);

        if (observers is null)
        {
            return Result.Fail(new Error($"Cannot find any related figures by a streetcode id: {request.StreetcodeId}"));
        }

        var targets = _repositoryWrapper.RelatedFigureRepository
            .FindAll(f => f.ObserverId == request.StreetcodeId).Select(t => t.Target);

        var relatedFigures = observers.Union(targets).Distinct();

        if (relatedFigures is null)
        {
            return Result.Fail(new Error($"Cannot find any related figures by a streetcode id: {request.StreetcodeId}"));
        }

        var mappedRelatedFigures = _mapper.Map<IEnumerable<RelatedFigureDTO>>(relatedFigures);
        return Result.Ok(mappedRelatedFigures);
    }
}