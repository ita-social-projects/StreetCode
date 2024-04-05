using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Enums;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.GetLastWithOffset
{
    public class GetLastWithOffsetHandler : IRequestHandler<GetLastWithOffsetQuery,
        Result<StreetcodeMainPageDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;
        private readonly IStringLocalizer<NoSharedResource> _stringLocalizerNo;

        public GetLastWithOffsetHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger, IStringLocalizer<NoSharedResource> stringLocalizerNo)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
            _stringLocalizerNo = stringLocalizerNo;
        }

        public async Task<Result<StreetcodeMainPageDTO>> Handle(GetLastWithOffsetQuery request, CancellationToken cancellationToken)
        {
            var streetcode = await _repositoryWrapper.StreetcodeRepository.GetFirstOrDefaultAsync(
                selector: default,
                predicate: sc => sc.Status == DAL.Enums.StreetcodeStatus.Published,
                include: src =>
                    src.Include(item => item.Text).Include(item => item.Images).ThenInclude(x => x.ImageDetails),
                orderByDESC: sc => sc.CreatedAt,
                offset: request.offset);

            if (streetcode != null)
            {
                const int keyNumOfImageToDisplay = (int)ImageAssigment.BlackAndWhite;
                streetcode.Images = streetcode.Images.Where(x => x.ImageDetails.Alt.Equals(keyNumOfImageToDisplay.ToString())).ToList();

                return Result.Ok(_mapper.Map<StreetcodeMainPageDTO>(streetcode));
            }

            string errorMsg = _stringLocalizerNo["NoStreetcodesExistNow"].Value;
            _logger.LogError(request, errorMsg);
            return Result.Fail(errorMsg);
        }
    }
}
