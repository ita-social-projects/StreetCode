using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.Interfaces.Streetcode;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.Services.Streetcode;

public class RelatedFigureService : IRelatedFigureService
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public RelatedFigureService(IMapper mapper, IRepositoryWrapper repositoryWrapper)
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
    }

    public async Task<IEnumerable<RelatedFigureDTO>> GetRelatedFiguresByStreetcodeId(int streetcodeId)
    {
        throw new NotImplementedException();

        /*var observerIds = _repositoryWrapper.RelatedFigureRepository
            .FindAll(f => f.TargetId == streetcodeId).Select(o => o.ObserverId);

        var targetIds = _repositoryWrapper.RelatedFigureRepository
            .FindAll(f => f.ObserverId == streetcodeId).Select(t => t.TargetId);

        var relatedFigureIds = observerIds.Union(targetIds).Distinct();

        if (relatedFigureIds is null)
        {
            return Result.Fail(new Error($"Cannot find any related figures by a streetcode id: {streetcodeId}"));
        }

        var relatedFigures = await _repositoryWrapper.StreetcodeRepository
            .GetAllAsync(
                predicate: sc => relatedFigureIds.Any(id => id == sc.Id),
                include: scl => scl
                    .Include(sc => sc.Images)
                    .Include(sc => sc.Tags));

        if (relatedFigures is null)
        {
            return Result.Fail(new Error($"Cannot find any related figures by a streetcode id: {streetcodeId}"));
        }

        var mappedRelatedFigures = _mapper.Map<IEnumerable<RelatedFigureDTO>>(relatedFigures);
        return Result.Ok(mappedRelatedFigures);*/
    }
}
