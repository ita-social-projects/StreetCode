using System.Security.Cryptography;
using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.BLL.Util;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Enums;
using Streetcode.DAL.Repositories.Interfaces.Base;

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

        public Task<Result<IEnumerable<StreetcodeMainPageDTO>>> Handle(GetPageOfStreetcodesMainPageQuery request, CancellationToken cancellationToken)
        {
            var streetcodes = _repositoryWrapper.StreetcodeRepository.GetAllPaginated(
                request.page,
                request.pageSize,
                predicate: sc => sc.Status == DAL.Enums.StreetcodeStatus.Published,
                include: src => src.Include(item => item.Text).Include(item => item.Images).ThenInclude(x => x.ImageDetails!),
                descendingSortKeySelector: sc => sc.CreatedAt)
                .Entities;

            if (streetcodes is not null && streetcodes.Any())
            {
                const int keyNumOfImageToDisplay = (int)ImageAssigment.Blackandwhite;
                foreach (var streetcode in streetcodes)
                {
                    streetcode.Images = streetcode.Images.Where(x => x.ImageDetails != null && x.ImageDetails.Alt!.Equals(keyNumOfImageToDisplay.ToString())).ToList();
                }

                return Task.FromResult(Result.Ok(_mapper.Map<IEnumerable<StreetcodeMainPageDTO>>(ShuffleStreetcodes(streetcodes))));
            }

            string errorMsg = _stringLocalizerNo["NoStreetcodesExistNow"].Value;
            _logger.LogError(request, errorMsg);
            return Task.FromResult(Result.Fail<IEnumerable<StreetcodeMainPageDTO>>(errorMsg));
        }

        private static List<StreetcodeContent> ShuffleStreetcodes(IEnumerable<StreetcodeContent> streetcodes)
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                return streetcodes.OrderBy(sc =>
                {
                    byte[] random = new byte[4];
                    rng.GetBytes(random);
                    return BitConverter.ToInt32(random, 0) & 0x7FFFFFFF;
                }).ToList();
            }
        }
    }
}
