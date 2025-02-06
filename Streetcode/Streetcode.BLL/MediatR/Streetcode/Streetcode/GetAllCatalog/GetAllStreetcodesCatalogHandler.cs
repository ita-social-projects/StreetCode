using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Streetcode.CatalogItem;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
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
        private readonly IStringLocalizer<NoSharedResource> _stringLocalizerNo;

        public GetAllStreetcodesCatalogHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger, IStringLocalizer<NoSharedResource> stringLocalizerNo)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
            _stringLocalizerNo = stringLocalizerNo;
        }

        public async Task<Result<IEnumerable<CatalogItem>>> Handle(GetAllStreetcodesCatalogQuery request, CancellationToken cancellationToken)
        {
            var streetcodes = await _repositoryWrapper.StreetcodeRepository.GetAllAsync(
                predicate: sc => sc.Status == StreetcodeStatus.Published,
                include: src => src.Include(item => item.Images).ThenInclude(x => x.ImageDetails!));

            if (streetcodes.Any())
            {
                const int keyNumOfImageToDisplay = (int)ImageAssigment.Blackandwhite;
                foreach (var streetcode in streetcodes)
                {
                    streetcode.Images = streetcode.Images.Where(x => x.ImageDetails != null && x.ImageDetails.Alt!.Equals(keyNumOfImageToDisplay.ToString())).ToList();
                }

                var skipped = streetcodes.Skip((request.page - 1) * request.count).Take(request.count);
                return Result.Ok(_mapper.Map<IEnumerable<CatalogItem>>(skipped));
            }

            string errorMsg = _stringLocalizerNo["NoStreetcodesExistNow"].Value;
            _logger.LogError(request, errorMsg);
            return Result.Fail(errorMsg);
        }
    }
}
