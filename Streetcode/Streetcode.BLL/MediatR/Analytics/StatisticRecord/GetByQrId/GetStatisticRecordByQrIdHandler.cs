using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Analytics;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Analytics.StatisticRecord.GetByQrId
{
    public class GetStatisticRecordByQrIdHandler : IRequestHandler<GetStatisticRecordByQrIdQuery, Result<StatisticRecordDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;
        private readonly IStringLocalizer<CannotMapSharedResource> _stringLocalizerCannotMap;

        public GetStatisticRecordByQrIdHandler(
            IMapper mapper,
            IRepositoryWrapper repositoryWrapper,
            IStringLocalizer<CannotMapSharedResource> stringLocalizerCannotMap,
            IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind)
        {
            _mapper = mapper;
            _repositoryWrapper = repositoryWrapper;
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
                return Result.Fail(new Error(_stringLocalizerCannotFind["CannotFindRecordWithQrId"].Value));
            }

            var statRecordDTO = _mapper.Map<StatisticRecordDTO>(statRecord);

            if(statRecordDTO == null)
            {
                return Result.Fail(new Error(_stringLocalizerCannotMap["CannotMapRecord"].Value));
            }

            return Result.Ok(statRecordDTO);
        }
    }
}
