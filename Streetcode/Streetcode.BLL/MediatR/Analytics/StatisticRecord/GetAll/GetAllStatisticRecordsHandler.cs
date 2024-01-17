using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.DTO.AdditionalContent.Subtitles;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Analytics;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Analytics.StatisticRecord.GetAll
{
    public class GetAllStatisticRecordsHandler : IRequestHandler<GetAllStatisticRecordsQuery, Result<IEnumerable<StatisticRecordDTO>>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;
        private readonly IStringLocalizer<CannotGetSharedResource> _stringLocalizerCannotGet;
        private readonly IStringLocalizer<CannotMapSharedResource> _stringLocalizerCannotMap;

        public GetAllStatisticRecordsHandler(
            IMapper mapper,
            IRepositoryWrapper repositoryWrapper,
            ILoggerService logger,
            IStringLocalizer<CannotGetSharedResource> stringLocalizerCannotGet,
            IStringLocalizer<CannotMapSharedResource> stringLocalizerCannotMap)
        {
            _mapper = mapper;
            _repositoryWrapper = repositoryWrapper;
            _logger = logger;
            _stringLocalizerCannotGet = stringLocalizerCannotGet;
            _stringLocalizerCannotMap = stringLocalizerCannotMap;
        }

        public async Task<Result<IEnumerable<StatisticRecordDTO>>> Handle(GetAllStatisticRecordsQuery request, CancellationToken cancellationToken)
        {
            var statisticRecords = await _repositoryWrapper.StatisticRecordRepository
                .GetAllAsync(include: sr => sr.Include(sr => sr.StreetcodeCoordinate));

            if(statisticRecords == null)
            {
                string errorMsg = _stringLocalizerCannotGet["CannotGetRecords"].Value;
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            var mappedEntities = _mapper.Map<IEnumerable<StatisticRecordDTO>>(statisticRecords);

            if(mappedEntities == null)
            {
                string errorMsg = _stringLocalizerCannotMap["CannotMapRecords"].Value;
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            return Result.Ok(mappedEntities.OrderByDescending((x) => x.Count).AsEnumerable());
        }
    }
}
