using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Streetcode.RelatedFigure;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Services.EntityAccessManager;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.RelatedFigure.GetByStreetcodeId;

public class GetRelatedFiguresByStreetcodeIdHandler : IRequestHandler<GetRelatedFigureByStreetcodeIdQuery, Result<IEnumerable<RelatedFigureDTO>?>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService _logger;
    private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;

    public GetRelatedFiguresByStreetcodeIdHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper, ILoggerService logger, IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind)
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
        _logger = logger;
        _stringLocalizerCannotFind = stringLocalizerCannotFind;
    }

    // If you use Rider instead of Visual Studio, for example, "SuppressMessage" attribute suppresses PossibleMultipleEnumeration warning
    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration", Justification = "Here is no sense to do materialization of query because of nested ToListAsync method in GetAllAsync method")]
    public async Task<Result<IEnumerable<RelatedFigureDTO>?>> Handle(GetRelatedFigureByStreetcodeIdQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<StreetcodeContent, bool>>? basePredicateForStreetcode = st => st.Id == request.StreetcodeId;
        var predicate = basePredicateForStreetcode.ExtendWithAccessPredicate(new StreetcodeAccessManager(), request.UserRole);

        var streetcodeContent = await _repositoryWrapper.StreetcodeRepository.GetFirstOrDefaultAsync(
            predicate: predicate,
            include: q => q.Include(s => s.Audio) !);

        if (streetcodeContent == null)
        {
            string errorMsg = _stringLocalizerCannotFind["CannotFindAnAudioWithTheCorrespondingStreetcodeId", request.StreetcodeId].Value;
            _logger.LogError(request, errorMsg);
            return Result.Fail(new Error(errorMsg));
        }

        var relatedFigureIds = GetRelatedFigureIdsByStreetcodeId(request.StreetcodeId);

        if (!relatedFigureIds.Any())
        {
            string message = "Returning empty enumerable of related figures";
            _logger.LogInformation(message);
            return Result.Ok(Enumerable.Empty<RelatedFigureDTO>());
        }

        var relatedFigures = await _repositoryWrapper.StreetcodeRepository.GetAllAsync(
          predicate: sc => relatedFigureIds.Any(id => id == sc.Id),
          include: scl => scl
              .Include(sc => sc.Images)
                .ThenInclude(img => img.ImageDetails)
              .Include(sc => sc.Tags));

        if (!relatedFigures.Any())
        {
            string errorMsg = _stringLocalizerCannotFind["CannotFindAnyRelatedFiguresByStreetcodeId", request.StreetcodeId].Value;
            _logger.LogError(request, errorMsg);

            return Result.Fail(new Error(errorMsg));
        }

        foreach(StreetcodeContent streetcode in relatedFigures)
        {
            if(streetcode.Images != null)
            {
                streetcode.Images = streetcode.Images.OrderBy(img => img.ImageDetails?.Alt).ToList();
            }
        }

        return Result.Ok<IEnumerable<RelatedFigureDTO>?>(_mapper.Map<IEnumerable<RelatedFigureDTO>>(relatedFigures));
    }

    private IQueryable<int> GetRelatedFigureIdsByStreetcodeId(int streetcodeId)
    {
        try
        {
            var observerIds = _repositoryWrapper.RelatedFigureRepository
            .FindAll(f => f.TargetId == streetcodeId).Select(o => o.ObserverId);

            var targetIds = _repositoryWrapper.RelatedFigureRepository
                .FindAll(f => f.ObserverId == streetcodeId).Select(t => t.TargetId);

            return observerIds.Union(targetIds).Distinct();
        }
        catch (ArgumentNullException)
        {
            return Enumerable.Empty<int>().AsQueryable();
        }
    }
}
