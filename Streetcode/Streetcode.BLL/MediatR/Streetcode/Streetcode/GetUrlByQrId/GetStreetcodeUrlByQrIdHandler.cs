using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.GetUrlByQrId
{
    public class GetStreetcodeUrlByQrIdHandler : IRequestHandler<GetStreetcodeUrlByQrIdQuery, Result<string>>
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;
        public GetStreetcodeUrlByQrIdHandler(IRepositoryWrapper repository, IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind)
        {
            _repository = repository;
            _stringLocalizerCannotFind = stringLocalizerCannotFind;
        }

        public async Task<Result<string>> Handle(GetStreetcodeUrlByQrIdQuery request, CancellationToken cancellationToken)
        {
            var statisticRecord = await _repository.StatisticRecordRepository
                .GetFirstOrDefaultAsync(
                predicate: (sr) => sr.QrId == request.qrId,
                include: (sr) => sr.Include((sr) => sr.StreetcodeCoordinate));

            if (statisticRecord == null)
            {
                return Result.Fail(new Error(_stringLocalizerCannotFind["CannotFindRecordWithQrId"].Value));
            }

            var streetcode = await _repository.StreetcodeRepository.GetFirstOrDefaultAsync((s) => s.Id == statisticRecord.StreetcodeCoordinate.StreetcodeId);

            if(streetcode == null)
            {
                return Result.Fail(new Error(_stringLocalizerCannotFind["CannotFindStreetcodeById"].Value));
            }

            return Result.Ok(streetcode.TransliterationUrl);
        }
    }
}
