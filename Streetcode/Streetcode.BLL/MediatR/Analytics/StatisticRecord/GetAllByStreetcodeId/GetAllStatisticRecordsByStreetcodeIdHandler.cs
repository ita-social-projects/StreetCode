using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.DTO.Analytics;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Analytics.StatisticRecord.GetAllByStreetcodeId
{
    internal class GetAllStatisticRecordsByStreetcodeIdHandler : IRequestHandler<GetAllStatisticRecordsByStreetcodeIdQuery, Result<IEnumerable<StatisticRecordDTO>>>
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public GetAllStatisticRecordsByStreetcodeIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repository = repositoryWrapper;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<StatisticRecordDTO>>> Handle(GetAllStatisticRecordsByStreetcodeIdQuery request, CancellationToken cancellationToken)
        {
            var statisticRecords = await _repository.StatisticRecordRepository.GetAllAsync(
                    predicate: st => st.StreetcodeCoordinate.StreetcodeId == request.streetcodeId,
                    include: st => st.Include(st => st.StreetcodeCoordinate));

            if (statisticRecords == null)
            {
                return Result.Fail(new Error("Cannot find any statistic for this streetcode"));
            }

            var statisticRecordsDTOs = _mapper.Map<IEnumerable<StatisticRecordDTO>>(statisticRecords);

            if (statisticRecordsDTOs == null)
            {
                return Result.Fail(new Error("Mapper is null"));
            }

            return Result.Ok(statisticRecordsDTOs);
        }
    }
}
