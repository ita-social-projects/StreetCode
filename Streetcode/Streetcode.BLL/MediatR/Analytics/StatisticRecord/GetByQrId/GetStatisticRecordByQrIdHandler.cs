using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.DTO.Analytics;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Analytics.StatisticRecord.GetByQrId
{
    public class GetStatisticRecordByQrIdHandler : IRequestHandler<GetStatisticRecordByQrIdQuery, Result<StatisticRecordDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;

        public GetStatisticRecordByQrIdHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper)
        {
            _mapper = mapper;
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<Result<StatisticRecordDTO>> Handle(GetStatisticRecordByQrIdQuery request, CancellationToken cancellationToken)
        {
            var statRecord = await _repositoryWrapper.StatisticRecordRepository
                .GetFirstOrDefaultAsync(
                predicate: (sr) => sr.QrId == request.qrId,
                include: (sr) => sr.Include((sr) => sr.StreetcodeCoordinate));

            if (statRecord == null)
            {
                return Result.Fail(new Error("Cannot find record with qrId"));
            }

            var statRecordDTO = _mapper.Map<StatisticRecordDTO>(statRecord);

            if(statRecordDTO == null)
            {
                return Result.Fail(new Error("Cannot map record"));
            }

            return Result.Ok(statRecordDTO);
        }
    }
}
