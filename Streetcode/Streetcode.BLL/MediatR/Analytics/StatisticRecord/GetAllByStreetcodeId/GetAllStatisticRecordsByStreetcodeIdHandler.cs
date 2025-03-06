using System.Linq.Expressions;
using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Analytics;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Services.EntityAccessManager;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Analytics.StatisticRecord.GetAllByStreetcodeId
{
    internal class GetAllStatisticRecordsByStreetcodeIdHandler : IRequestHandler<GetAllStatisticRecordsByStreetcodeIdQuery, Result<IEnumerable<StatisticRecordDTO>>>
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;
        private readonly ILoggerService _logger;
        private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;
        private readonly IStringLocalizer<CannotMapSharedResource> _stringLocalizerCannotMap;

        public GetAllStatisticRecordsByStreetcodeIdHandler(
            IRepositoryWrapper repositoryWrapper,
            IMapper mapper,
            ILoggerService logger,
            IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind,
            IStringLocalizer<CannotMapSharedResource> stringLocalizerCannotMap)
        {
            _repository = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
            _stringLocalizerCannotFind = stringLocalizerCannotFind;
            _stringLocalizerCannotMap = stringLocalizerCannotMap;
        }

        public async Task<Result<IEnumerable<StatisticRecordDTO>>> Handle(GetAllStatisticRecordsByStreetcodeIdQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<StreetcodeContent, bool>>? basePredicate = str => str.Id == request.StreetcodeId;
            var predicate = basePredicate.ExtendWithAccessPredicate(new StreetcodeAccessManager(), request.UserRole);

            var isStreetcodeExists = await _repository.StreetcodeRepository.FindAll(predicate: predicate).AnyAsync(cancellationToken);

            if (!isStreetcodeExists)
            {
                string errorMsg = _stringLocalizerCannotFind["CannotFindAnyStreetcodeWithCorrespondingId", request.StreetcodeId].Value;
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            var statisticRecords = await _repository.StatisticRecordRepository.GetAllAsync(
                    predicate: st => st.StreetcodeCoordinate.StreetcodeId == request.StreetcodeId,
                    include: st => st.Include(st => st.StreetcodeCoordinate));

            if (statisticRecords is null)
            {
                string errorMsg = _stringLocalizerCannotFind["CannotFindRecordWithStreetcodeId", request.StreetcodeId];
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            var statisticRecordsDTOs = _mapper.Map<IEnumerable<StatisticRecordDTO>>(statisticRecords);

            if (statisticRecordsDTOs is null)
            {
                string errorMsg = _stringLocalizerCannotMap["CannotMapRecord"];
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            return Result.Ok(statisticRecordsDTOs);
        }
    }
}
