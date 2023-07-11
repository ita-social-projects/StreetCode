using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.GetUrlByQrId
{
    public class GetStreetcodeUrlByQrIdHandler : IRequestHandler<GetStreetcodeUrlByQrIdQuery, Result<string>>
    {
        private readonly IRepositoryWrapper _repository;
        private readonly ILoggerService _logger;
        public GetStreetcodeUrlByQrIdHandler(IRepositoryWrapper repository, ILoggerService logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<Result<string>> Handle(GetStreetcodeUrlByQrIdQuery request, CancellationToken cancellationToken)
        {
            var statisticRecord = await _repository.StatisticRecordRepository
                .GetFirstOrDefaultAsync(
                predicate: (sr) => sr.QrId == request.qrId,
                include: (sr) => sr.Include((sr) => sr.StreetcodeCoordinate));

            if (statisticRecord == null)
            {
                const string errorMsg = "Cannot find record by qrid";
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            var streetcode = await _repository.StreetcodeRepository.GetFirstOrDefaultAsync((s) => s.Id == statisticRecord.StreetcodeCoordinate.StreetcodeId);

            if(streetcode == null)
            {
                const string errorMsg = "Cannot find streetcode by id";
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            return Result.Ok(streetcode.TransliterationUrl);
        }
    }
}
