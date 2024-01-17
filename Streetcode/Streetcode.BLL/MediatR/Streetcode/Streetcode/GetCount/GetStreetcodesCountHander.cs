using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Enums;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.GetCount
{
    public class GetStreetcodesCountHander : IRequestHandler<GetStreetcodesCountQuery,
        Result<int>>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;
        private readonly IStringLocalizer<NoSharedResource> _stringLocalizerNo;

        public GetStreetcodesCountHander(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger, IStringLocalizer<NoSharedResource> stringLocalizerNo)
        {
            _repositoryWrapper = repositoryWrapper;
            _logger = logger;
            _stringLocalizerNo = stringLocalizerNo;
        }

        public async Task<Result<int>> Handle(GetStreetcodesCountQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<StreetcodeContent> streetcodes;

            if (request.onlyPublished)
            {
                streetcodes = await _repositoryWrapper.StreetcodeRepository
                    .GetAllAsync(s => s.Status == StreetcodeStatus.Published);
            }
            else
            {
                streetcodes = await _repositoryWrapper.StreetcodeRepository.GetAllAsync();
            }

            if (streetcodes != null)
            {
                return Result.Ok(streetcodes.Count());
            }

            string errorMsg = _stringLocalizerNo["NoStreetcodesExistNow"].Value;
            _logger.LogError(request, errorMsg);
            return Result.Fail(errorMsg);
        }
    }
}
