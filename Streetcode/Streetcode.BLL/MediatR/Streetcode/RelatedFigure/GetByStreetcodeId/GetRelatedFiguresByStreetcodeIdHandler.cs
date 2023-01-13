using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.DAL.Entities.AdditionalContent;
using Streetcode.DAL.Entities.Streetcode;
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
        IEnumerable<Tag>? tags = await _repositoryWrapper.TagRepository
            .GetAllAsync(t => t.Streetcodes.Any(sc => sc.Id == request.StreetcodeId));

        if (tags is null)
        {
            return Result.Fail(new Error($"Cannot find any tags by a streetcode id: {request.StreetcodeId}"));
        }

        var relatedFigureIds = GetRelatedFigureIdsByStreetcodeId(request.StreetcodeId);

        if (relatedFigureIds is null)
        {
            return Result.Fail(new Error($"Cannot find any related figures by a streetcode id: {request.StreetcodeId}"));
        }

        var relatedFigures = await _repositoryWrapper.StreetcodeRepository
            .GetAllAsync(
                predicate: sc => relatedFigureIds.Any(id => id == sc.Id),
                include: scl => scl
                    .Include(sc => sc.Images)
                    .Include(sc => sc.Tags));

        if (relatedFigures is null)
        {
            return Result.Fail(new Error($"Cannot find any related figures by a streetcode id: {request.StreetcodeId}"));
        }

        IntercectTags(relatedFigures, tags);

        var mappedRelatedFigures = _mapper.Map<IEnumerable<RelatedFigureDTO>>(relatedFigures);
        return Result.Ok(mappedRelatedFigures);
    }

    private IQueryable<int> GetRelatedFigureIdsByStreetcodeId(int StreetcodeId)
    {
        var observerIds = _repositoryWrapper.RelatedFigureRepository
            .FindAll(f => f.TargetId == StreetcodeId).Select(o => o.ObserverId);

        var targetIds = _repositoryWrapper.RelatedFigureRepository
            .FindAll(f => f.ObserverId == StreetcodeId).Select(t => t.TargetId);

        return observerIds.Union(targetIds).Distinct();
    }

    private void IntercectTags(IEnumerable<StreetcodeContent> relatedFigures, IEnumerable<Tag> tags)
    {
        relatedFigures.ToList()
            .ForEach(f => f.Tags = f.Tags.Where(t => tags.Any(tg => tg.Id == t.Id)).ToList());
    }
}