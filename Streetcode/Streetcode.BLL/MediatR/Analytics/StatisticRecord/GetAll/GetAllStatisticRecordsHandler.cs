using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Analytics;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Analytics.StatisticRecord.GetAll
{
    public class GetAllStatisticRecordsHandler : IRequestHandler<GetAllStatisticRecordsQuery, Result<IEnumerable<StatisticRecordDTO>>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IStringLocalizer<CannotGetSharedResource> _stringLocalizerCannotGet;
        private readonly IStringLocalizer<CannotMapSharedResource> _stringLocalizerCannotMap;

        public GetAllStatisticRecordsHandler(
            IMapper mapper,
            IRepositoryWrapper repositoryWrapper,
            IStringLocalizer<CannotGetSharedResource> stringLocalizerCannotGet,
            IStringLocalizer<CannotMapSharedResource> stringLocalizerCannotMap)
        {
            _mapper = mapper;
            _repositoryWrapper = repositoryWrapper;
            _stringLocalizerCannotGet = stringLocalizerCannotGet;
            _stringLocalizerCannotMap = stringLocalizerCannotMap;
        }

        public async Task<Result<IEnumerable<StatisticRecordDTO>>> Handle(GetAllStatisticRecordsQuery request, CancellationToken cancellationToken)
        {
            var statisticRecords = await _repositoryWrapper.StatisticRecordRepository
                .GetAllAsync(include: sr => sr.Include(sr => sr.StreetcodeCoordinate));

            if(statisticRecords == null)
            {
                return Result.Fail(new Error(_stringLocalizerCannotGet["CannotGetRecords"].Value));
            }

            var mappedEntities = _mapper.Map<IEnumerable<StatisticRecordDTO>>(statisticRecords);

            if(mappedEntities == null)
            {
                return Result.Fail(new Error(_stringLocalizerCannotMap["CannotMapRecords"].Value));
            }

            var sortedEntities = mappedEntities.OrderByDescending((x) => x.Count).AsEnumerable();

            return Result.Ok(sortedEntities);
        }
    }
}
