using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.DTO.AdditionalContent.Subtitles;
using Streetcode.BLL.DTO.Analytics;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Analytics.StatisticRecord.GetAll
{
    public class GetAllStatisticRecordsHandler : IRequestHandler<GetAllStatisticRecordsQuery, Result<IEnumerable<StatisticRecordDTO>>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;

        public GetAllStatisticRecordsHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper, ILoggerService logger)
        {
            _mapper = mapper;
            _repositoryWrapper = repositoryWrapper;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<StatisticRecordDTO>>> Handle(GetAllStatisticRecordsQuery request, CancellationToken cancellationToken)
        {
            var statisticRecords = await _repositoryWrapper.StatisticRecordRepository
                .GetAllAsync(include: sr => sr.Include(sr => sr.StreetcodeCoordinate));

            if(statisticRecords == null)
            {
                const string errorMsg = "Cannot get records";
                _logger?.LogError("GetAllStatisticRecordsQuery handled with an error");
                _logger?.LogError(errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            var mappedEntities = _mapper.Map<IEnumerable<StatisticRecordDTO>>(statisticRecords);

            if(mappedEntities == null)
            {
                const string errorMsg = "Cannot map records";
                _logger?.LogError("GetAllStatisticRecordsQuery handled with an error");
                _logger?.LogError(errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            var sortedEntities = mappedEntities.OrderByDescending((x) => x.Count).AsEnumerable();
            _logger?.LogInformation($"GetAllStatisticRecordsQuery handled successfully");
            _logger?.LogInformation($"Retrieved {sortedEntities.Count()} statisctics");
            return Result.Ok(sortedEntities);
        }
    }
}
