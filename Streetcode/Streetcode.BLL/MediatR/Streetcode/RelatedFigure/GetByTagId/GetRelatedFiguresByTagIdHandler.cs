using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.DTO.Streetcode.RelatedFigure;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.MediatR.Streetcode.RelatedFigure.GetByTagId
{
    public class GetRelatedFiguresByTagIdHandler : IRequestHandler<GetRelatedFiguresByTagIdQuery, Result<IEnumerable<RelatedFigureDTO>?>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;

        public GetRelatedFiguresByTagIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
        }

        // If you use Rider instead of Visual Studio, for example, "SuppressMessage" attribute suppresses PossibleMultipleEnumeration warning
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration", Justification = "Here is no sense to do materialization of query because of nested ToListAsync method in GetAllAsync method")]
        public async Task<Result<IEnumerable<RelatedFigureDTO>>> Handle(GetRelatedFiguresByTagIdQuery request, CancellationToken cancellationToken)
        {
            var streetcodes = await _repositoryWrapper.StreetcodeRepository
                .GetAllAsync(
                predicate: sc => sc.Status == DAL.Enums.StreetcodeStatus.Published &&
                  sc.Tags.Select(t => t.Id).Any(tag => tag == request.TagId),
                include: scl => scl
                    .Include(sc => sc.Images).ThenInclude(x => x.ImageDetails)
                    .Include(sc => sc.Tags));

            if (!streetcodes.Any())
            {
                string message = "Returning empty enumerable of related figures";
                _logger.LogInformation(message);
                return Result.Ok(Enumerable.Empty<RelatedFigureDTO>());
            }

            const int blackAndWhiteImageAssignmentKey = (int)ImageAssigment.Blackandwhite;
            foreach (var streetcode in streetcodes)
            {
                streetcode.Images = streetcode.Images.Where(x => x.ImageDetails != null && x.ImageDetails.Alt!.Equals(blackAndWhiteImageAssignmentKey.ToString())).ToList();
            }

            return Result.Ok(_mapper.Map<IEnumerable<RelatedFigureDTO>>(streetcodes));
        }
    }
}
