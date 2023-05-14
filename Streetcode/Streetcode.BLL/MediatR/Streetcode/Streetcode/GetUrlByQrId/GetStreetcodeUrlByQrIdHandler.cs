using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.GetUrlByQrId
{
    public class GetStreetcodeUrlByQrIdHandler : IRequestHandler<GetStreetcodeUrlByQrIdQuery, Result<string>>
    {
        private readonly IRepositoryWrapper _repository;
        public GetStreetcodeUrlByQrIdHandler(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        public async Task<Result<string>> Handle(GetStreetcodeUrlByQrIdQuery request, CancellationToken cancellationToken)
        {
            var statisticRecord = await _repository.StatisticRecordRepository
                .GetFirstOrDefaultAsync(
                predicate: (sr) => sr.QrId == request.qrId,
                include: (sr) => sr.Include((sr) => sr.StreetcodeCoordinate));

            if (statisticRecord == null)
            {
                return Result.Fail(new Error("Cannot find record by qrid"));
            }

            var streetcode = await _repository.StreetcodeRepository.GetFirstOrDefaultAsync((s) => s.Id == statisticRecord.StreetcodeCoordinate.StreetcodeId);

            if(streetcode == null)
            {
                return Result.Fail(new Error("Cannot find streetcode by id"));
            }

            return Result.Ok(streetcode.TransliterationUrl);
        }
    }
}
