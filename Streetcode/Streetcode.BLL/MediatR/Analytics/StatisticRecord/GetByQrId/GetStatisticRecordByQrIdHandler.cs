using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.DTO.Analytics;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Analytics.StatisticRecord.GetByQrId
{
    public class GetStatisticRecordByQrIdHandler : IRequestHandler<GetStatisticRecordByQrIdQuery, Result<StatisticRecordDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;

        public GetStatisticRecordByQrIdHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper, ILoggerService logger)
        {
            _mapper = mapper;
            _repositoryWrapper = repositoryWrapper;
            _logger = logger;
        }

        public async Task<Result<StatisticRecordDTO>> Handle(GetStatisticRecordByQrIdQuery request, CancellationToken cancellationToken)
        {
            var statRecord = await _repositoryWrapper.StatisticRecordRepository
                .GetFirstOrDefaultAsync(
                predicate: (sr) => sr.QrId == request.qrId,
                include: (sr) => sr.Include((sr) => sr.StreetcodeCoordinate));

            if (statRecord == null)
            {
                const string errorMsg = $"Cannot find record with qrId";
                _logger?.LogError("GetStatisticRecordByQrIdQuery handled with an error");
                _logger?.LogError(errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            var statRecordDTO = _mapper.Map<StatisticRecordDTO>(statRecord);

            if(statRecordDTO == null)
            {
                const string errorMsg = $"Cannot map record";
                _logger?.LogError("GetStatisticRecordByQrIdQuery handled with an error");
                _logger?.LogError(errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            _logger?.LogInformation($"GetStatisticRecordByQrIdQuery handled successfully");
            return Result.Ok(statRecordDTO);
        }
    }
}
