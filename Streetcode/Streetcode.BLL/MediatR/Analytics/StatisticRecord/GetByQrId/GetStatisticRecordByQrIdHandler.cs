using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Analytics;
using Streetcode.BLL.SharedResource;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Analytics.StatisticRecord.GetByQrId
{
    public class GetStatisticRecordByQrIdHandler : IRequestHandler<GetStatisticRecordByQrIdQuery, Result<StatisticRecordDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;
        private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;
        private readonly IStringLocalizer<CannotMapSharedResource> _stringLocalizerCannotMap;

        public GetStatisticRecordByQrIdHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper, ILoggerService logger)
        public GetStatisticRecordByQrIdHandler(
            IMapper mapper,
            IRepositoryWrapper repositoryWrapper,
            ILoggerService logger,
            IStringLocalizer<CannotMapSharedResource> stringLocalizerCannotMap,
            IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind)
        {
            _mapper = mapper;
            _repositoryWrapper = repositoryWrapper;
            _logger = logger;
            _stringLocalizerCannotMap = stringLocalizerCannotMap;
            _stringLocalizerCannotFind = stringLocalizerCannotFind;
        }

        public async Task<Result<StatisticRecordDTO>> Handle(GetStatisticRecordByQrIdQuery request, CancellationToken cancellationToken)
        {
            var statRecord = await _repositoryWrapper.StatisticRecordRepository
                .GetFirstOrDefaultAsync(
                predicate: (sr) => sr.QrId == request.qrId,
                include: (sr) => sr.Include((sr) => sr.StreetcodeCoordinate));

            if (statRecord == null)
            {
                const string errorMsg = _stringLocalizerCannotFind["CannotFindRecordWithQrId"].Value;
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            var statRecordDTO = _mapper.Map<StatisticRecordDTO>(statRecord);

            if(statRecordDTO == null)
            {
                const string errorMsg = _stringLocalizerCannotMap["CannotMapRecord"].Value;
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            return Result.Ok(statRecordDTO);
        }
    }
}
