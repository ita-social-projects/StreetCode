using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Streetcode.RelatedFigure;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.RelatedFigure.GetByStreetcodeId;

public class GetRelatedFiguresByStreetcodeIdHandler : IRequestHandler<GetRelatedFigureByStreetcodeIdQuery, Result<IEnumerable<RelatedFigureDTO>>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;

    public GetRelatedFiguresByStreetcodeIdHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper, IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind)
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
        _stringLocalizerCannotFind = stringLocalizerCannotFind;
    }

    public async Task<Result<IEnumerable<RelatedFigureDTO>>> Handle(GetRelatedFigureByStreetcodeIdQuery request, CancellationToken cancellationToken)
    {
        var relatedFigureIds = GetRelatedFigureIdsByStreetcodeId(request.StreetcodeId);

        if (relatedFigureIds is null)
        {
            return Result.Fail(new Error(_stringLocalizerCannotFind["CannotFindAnyRelatedFiguresByStreetcodeId", request.StreetcodeId].Value));
        }

        var relatedFigures = await _repositoryWrapper.StreetcodeRepository.GetAllAsync(
          predicate: sc => relatedFigureIds.Any(id => id == sc.Id) && sc.Status == DAL.Enums.StreetcodeStatus.Published,
          include: scl => scl.Include(sc => sc.Images).Include(sc => sc.Tags));

        if (relatedFigures is null)
        {
            return Result.Fail(new Error(_stringLocalizerCannotFind["CannotFindAnyRelatedFiguresByStreetcodeId", request.StreetcodeId].Value));
        }

        var mappedRelatedFigures = _mapper.Map<IEnumerable<RelatedFigureDTO>>(relatedFigures);
        return Result.Ok(mappedRelatedFigures);
    }

    private IQueryable<int> GetRelatedFigureIdsByStreetcodeId(int StreetcodeId)
    {
        try
        {
            var observerIds = _repositoryWrapper.RelatedFigureRepository
            .FindAll(f => f.TargetId == StreetcodeId).Select(o => o.ObserverId);

            var targetIds = _repositoryWrapper.RelatedFigureRepository
                .FindAll(f => f.ObserverId == StreetcodeId).Select(t => t.TargetId);

            return observerIds.Union(targetIds).Distinct();
        }
        catch (ArgumentNullException)
        {
            return null;
        }
    }
}