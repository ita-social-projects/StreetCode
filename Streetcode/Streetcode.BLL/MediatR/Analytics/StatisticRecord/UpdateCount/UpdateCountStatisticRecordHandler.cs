using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.Interfaces.Logging;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Analytics.StatisticRecord.UpdateCount
{
    public class UpdateCountStatisticRecordHandler : IRequestHandler<UpdateCountStatisticRecordCommand, Result<Unit>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;
        private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;
        private readonly IStringLocalizer<CannotSaveSharedResource> _stringLocalizerCannotSave;

        public UpdateCountStatisticRecordHandler(
            IMapper mapper,
            IRepositoryWrapper repositoryWrapper,
            ILoggerService logger,
            IStringLocalizer<CannotSaveSharedResource> stringLocalizerCannotSave,
            IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind)
        {
            _mapper = mapper;
            _repositoryWrapper = repositoryWrapper;
            _logger = logger;
            _stringLocalizerCannotSave = stringLocalizerCannotSave;
            _stringLocalizerCannotFind = stringLocalizerCannotFind;
        }

        public async Task<Result<Unit>> Handle(UpdateCountStatisticRecordCommand request, CancellationToken cancellationToken)
        {
            var statRecord = await _repositoryWrapper.StatisticRecordRepository.GetFirstOrDefaultAsync(
                predicate: (sr) => sr.QrId == request.qrId);

            if (statRecord == null)
            {
                string errorMsg = _stringLocalizerCannotFind["CannotFindRecordWithQrId"].Value;
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            statRecord.Count++;

            _repositoryWrapper.StatisticRecordRepository.Update(statRecord);

            var resultIsSuccess = _repositoryWrapper.SaveChanges() > 0;

            if (!resultIsSuccess)
            {
                string errorMsg = _stringLocalizerCannotSave["CannotSaveTheData"].Value;
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            return Result.Ok(Unit.Value);
        }
    }
}
