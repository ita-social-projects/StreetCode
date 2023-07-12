using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.DTO.Analytics;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Analytics.StatisticRecord.GetAllByStreetcodeId
{
    internal class GetAllStatisticRecordsByStreetcodeIdHandler : IRequestHandler<GetAllStatisticRecordsByStreetcodeIdQuery, Result<IEnumerable<StatisticRecordDTO>>>
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;
        private readonly ILoggerService _logger;

        public GetAllStatisticRecordsByStreetcodeIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger)
        {
            _repository = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<StatisticRecordDTO>>> Handle(GetAllStatisticRecordsByStreetcodeIdQuery request, CancellationToken cancellationToken)
        {
            var statisticRecords = await _repository.StatisticRecordRepository.GetAllAsync(
                    predicate: st => st.StreetcodeCoordinate.StreetcodeId == request.streetcodeId,
                    include: st => st.Include(st => st.StreetcodeCoordinate));

            if (statisticRecords == null)
            {
                const string errorMsg = "Cannot find any statistic for this streetcode";
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            var statisticRecordsDTOs = _mapper.Map<IEnumerable<StatisticRecordDTO>>(statisticRecords);

            if (statisticRecordsDTOs == null)
            {
                const string errorMsg = "Mapper is null";
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            return Result.Ok(statisticRecordsDTOs);
        }
    }
}
