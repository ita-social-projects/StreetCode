using System.Linq.Expressions;
using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Services.EntityAccessManagerService;
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
            Expression<Func<StreetcodeContent, bool>>? basePredicate = null;
            if (request.onlyPublished)
            {
                basePredicate = s => s.Status == StreetcodeStatus.Published;
            }

            var predicate = basePredicate.ExtendWithAccessPredicate(new StreetcodeAccessManager(), request.userRole);

            var streetcodes = await _repositoryWrapper.StreetcodeRepository.GetAllAsync(predicate: predicate);

            if (streetcodes.Any())
            {
                return Result.Ok(streetcodes.Count());
            }

            string errorMsg = _stringLocalizerNo["NoStreetcodesExistNow"].Value;
            _logger.LogError(request, errorMsg);
            return Result.Fail(errorMsg);
        }
    }
}
