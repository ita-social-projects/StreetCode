using System.Linq.Expressions;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Services.EntityAccessManager;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.GetUrlByQrId;

public class GetStreetcodeUrlByQrIdHandler : IRequestHandler<GetStreetcodeUrlByQrIdQuery, Result<string>>
{
    private readonly IRepositoryWrapper _repository;
    private readonly ILoggerService _logger;
    private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;
    public GetStreetcodeUrlByQrIdHandler(IRepositoryWrapper repository, ILoggerService logger, IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind)
    {
        _repository = repository;
        _logger = logger;
        _stringLocalizerCannotFind = stringLocalizerCannotFind;
    }

    public async Task<Result<string>> Handle(GetStreetcodeUrlByQrIdQuery request, CancellationToken cancellationToken)
    {
        var statisticRecord = await _repository.StatisticRecordRepository
            .GetFirstOrDefaultAsync(
                predicate: (sr) => sr.QrId == request.QrId,
                include: (sr) => sr.Include((record) => record.StreetcodeCoordinate));

        if (statisticRecord == null)
        {
            var errorMsg = _stringLocalizerCannotFind["CannotFindRecordWithQrId"].Value;
            _logger.LogError(request, errorMsg);
            return Result.Fail(new Error(errorMsg));
        }

        Expression<Func<StreetcodeContent, bool>>? basePredicate = s => s.Id == statisticRecord.StreetcodeCoordinate.StreetcodeId;
        var predicate = basePredicate.ExtendWithAccessPredicate(new StreetcodeAccessManager(), request.UserRole);

        var streetcode = await _repository.StreetcodeRepository.GetFirstOrDefaultAsync(predicate: predicate);

        if(streetcode == null)
        {
            var errorMsg = _stringLocalizerCannotFind["CannotFindStreetcodeById"].Value;
            _logger.LogError(request, errorMsg);
            return Result.Fail(new Error(errorMsg));
        }

        return Result.Ok(streetcode.TransliterationUrl!);
    }
}