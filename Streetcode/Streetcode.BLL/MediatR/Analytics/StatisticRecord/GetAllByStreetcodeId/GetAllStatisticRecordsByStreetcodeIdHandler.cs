using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Analytics;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Analytics.StatisticRecord.GetAllByStreetcodeId
{
    public class GetAllStatisticRecordsByStreetcodeIdHandler : IRequestHandler<GetAllStatisticRecordsByStreetcodeIdQuery, Result<IEnumerable<StatisticRecordDTO>>>
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;
        private readonly ILoggerService _logger;
        private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;

        public GetAllStatisticRecordsByStreetcodeIdHandler(
            IRepositoryWrapper repositoryWrapper,
            IMapper mapper,
            ILoggerService logger,
            IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind)
        {
            _repository = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
            _stringLocalizerCannotFind = stringLocalizerCannotFind;
        }

        public async Task<Result<IEnumerable<StatisticRecordDTO>>> Handle(GetAllStatisticRecordsByStreetcodeIdQuery request, CancellationToken cancellationToken)
        {
            var statisticRecords = await _repository.StatisticRecordRepository.GetAllAsync(
                    predicate: st => st.StreetcodeCoordinate.StreetcodeId == request.streetcodeId,
                    include: st => st.Include(st => st.StreetcodeCoordinate));

            if (!statisticRecords.Any())
            {
                string errorMsg = _stringLocalizerCannotFind["CannotFindRecordWithStreetcodeId", request.streetcodeId];
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            var statisticRecordsDTOs = _mapper.Map<IEnumerable<StatisticRecordDTO>>(statisticRecords);

            return Result.Ok(statisticRecordsDTOs);
        }
    }
}
