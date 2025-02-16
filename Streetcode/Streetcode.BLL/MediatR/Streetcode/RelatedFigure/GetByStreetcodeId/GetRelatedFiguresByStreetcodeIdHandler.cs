using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.DTO.Streetcode.RelatedFigure;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.RelatedFigure.GetByStreetcodeId;

public class GetRelatedFiguresByStreetcodeIdHandler : IRequestHandler<GetRelatedFigureByStreetcodeIdQuery, Result<IEnumerable<RelatedFigureDTO>>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService _logger;

    public GetRelatedFiguresByStreetcodeIdHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper, ILoggerService logger)
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
        _logger = logger;
    }

    // If you use Rider instead of Visual Studio, for example, "SuppressMessage" attribute suppresses PossibleMultipleEnumeration warning
    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration", Justification = "Here is no sense to do materialization of query because of nested ToListAsync method in GetAllAsync method")]
    public async Task<Result<IEnumerable<RelatedFigureDTO>>> Handle(GetRelatedFigureByStreetcodeIdQuery request, CancellationToken cancellationToken)
    {
        var relatedFigureIds = GetRelatedFigureIdsByStreetcodeId(request.StreetcodeId);

        if (!relatedFigureIds.Any())
        {
            string message = "Returning empty enumerable of related figures";
            _logger.LogInformation(message);
            return Result.Ok(Enumerable.Empty<RelatedFigureDTO>());
        }

        var relatedFigures = await _repositoryWrapper.StreetcodeRepository.GetAllAsync(
          predicate: sc => relatedFigureIds.Any(id => id == sc.Id) && sc.Status == DAL.Enums.StreetcodeStatus.Published,
          include: scl => scl.Include(sc => sc.Images).ThenInclude(img => img.ImageDetails)
                             .Include(sc => sc.Tags));

        if (!relatedFigureIds.Any())
        {
            string message = "Returning empty enumerable of related figures";
            _logger.LogInformation(message);
            return Result.Ok(Enumerable.Empty<RelatedFigureDTO>());
        }

        foreach(StreetcodeContent streetcode in relatedFigures)
        {
            streetcode.Images = streetcode.Images.OrderBy(img => img.ImageDetails?.Alt).ToList();
        }

        var relatedFigureDto = _mapper.Map<IEnumerable<RelatedFigureDTO>>(relatedFigures);

        return Result.Ok(relatedFigureDto);
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
