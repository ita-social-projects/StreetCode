using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.DTO.Streetcode.CatalogItem;
using Streetcode.BLL.DTO.Streetcode.RelatedFigure;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Enums;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.GetAllCatalog
{
  public class GetAllStreetcodesCatalogHandler : IRequestHandler<GetAllStreetcodesCatalogQuery,
        Result<IEnumerable<CatalogItem>>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;

        public GetAllStreetcodesCatalogHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<CatalogItem>>> Handle(GetAllStreetcodesCatalogQuery request, CancellationToken cancellationToken)
        {
            var streetcodes = await _repositoryWrapper.StreetcodeRepository.GetAllAsync(
                predicate: sc => sc.Status == DAL.Enums.StreetcodeStatus.Published,
                include: src => src.Include(item => item.Images).ThenInclude(x => x.ImageDetails));

            if (streetcodes != null)
            {
                const int keyNumOfImageToDisplay = (int)ImageAssigment.Blackandwhite;
                foreach (var streetcode in streetcodes)
                {
                    streetcode.Images = streetcode.Images.Where(x => x.ImageDetails.Alt.Equals(keyNumOfImageToDisplay.ToString())).ToList();
                }

                var skipped = streetcodes.Skip((request.page - 1) * request.count).Take(request.count);
                return Result.Ok(_mapper.Map<IEnumerable<CatalogItem>>(skipped));
            }

            const string errorMsg = $"Cannot find any subtitles";
            _logger.LogError(request, errorMsg);
            return Result.Fail(errorMsg);
        }
    }
}
