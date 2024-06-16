using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.BLL.Util;
using Streetcode.DAL.Enums;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System.Security.Cryptography;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.GetPageMainPage
{
    public class GetPageOfStreetcodesMainPageHandler : IRequestHandler<GetPageOfStreetcodesMainPageQuery,
        Result<IEnumerable<StreetcodeMainPageDTO>>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;
        private readonly IStringLocalizer<NoSharedResource> _stringLocalizerNo;

        public GetPageOfStreetcodesMainPageHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger, IStringLocalizer<NoSharedResource> stringLocalizerNo)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
            _stringLocalizerNo = stringLocalizerNo;
        }

        public async Task<Result<IEnumerable<StreetcodeMainPageDTO>>> Handle(GetPageOfStreetcodesMainPageQuery request, CancellationToken cancellationToken)
        {
            var streetcodes = (await _repositoryWrapper.StreetcodeRepository.GetAllAsync(
                predicate: sc => sc.Status == DAL.Enums.StreetcodeStatus.Published,
                include: src => src.Include(item => item.Text).Include(item => item.Images).ThenInclude(x => x.ImageDetails)))
                .OrderByDescending(sc => sc.CreatedAt);

            if (streetcodes != null)
            {
                const int keyNumOfImageToDisplay = (int)ImageAssigment.Blackandwhite;
                foreach (var streetcode in streetcodes)
                {
                    streetcode.Images = streetcode.Images.Where(x => x.ImageDetails != null && x.ImageDetails.Alt.Equals(keyNumOfImageToDisplay.ToString())).ToList();
                }

                var streetcodesPaginated = streetcodes.Paginate(request.page, request.pageSize);

                var shuffledStreetcodes = streetcodes.OrderBy(sc =>
                {
                    using var rng = RandomNumberGenerator.Create();
                    byte[] random = new byte[4];
                    rng.GetBytes(random);
                    return BitConverter.ToInt32(random, 0) & 0x7FFFFFFF;
                });

                return Result.Ok(_mapper.Map<IEnumerable<StreetcodeMainPageDTO>>(shuffledStreetcodes));
            }

            string errorMsg = _stringLocalizerNo["NoStreetcodesExistNow"].Value;
            _logger.LogError(request, errorMsg);
            return Result.Fail(errorMsg);
        }
    }
}