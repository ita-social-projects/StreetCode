using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.DTO.Analytics;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Analytics.StatisticRecord.GetAll
{
    public class GetAllStatisticRecordsHandler : IRequestHandler<GetAllStatisticRecordsQuery, Result<IEnumerable<StatisticRecordDTO>>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;

        public GetAllStatisticRecordsHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper)
        {
            _mapper = mapper;
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<Result<IEnumerable<StatisticRecordDTO>>> Handle(GetAllStatisticRecordsQuery request, CancellationToken cancellationToken)
        {
            var statisticRecords = await _repositoryWrapper.StatisticRecordRepository
                .GetAllAsync(include: sr => sr.Include(sr => sr.StreetcodeCoordinate));

            if(statisticRecords == null)
            {
                return Result.Fail(new Error("Cannot get records"));
            }

            var mappedEntities = _mapper.Map<IEnumerable<StatisticRecordDTO>>(statisticRecords);

            if(mappedEntities == null)
            {
                return Result.Fail(new Error("Cannot map records"));
            }

            return Result.Ok(mappedEntities);
        }
    }
}
