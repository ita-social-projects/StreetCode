using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Analytics.StatisticRecord.UpdateCount
{
    public class UpdateCountStatisticRecordHandler : IRequestHandler<UpdateCountStatisticRecordCommand, Result<Unit>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;

        public UpdateCountStatisticRecordHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper, ILoggerService logger)
        {
            _mapper = mapper;
            _repositoryWrapper = repositoryWrapper;
            _logger = logger;
        }

        public async Task<Result<Unit>> Handle(UpdateCountStatisticRecordCommand request, CancellationToken cancellationToken)
        {
            var statRecord = await _repositoryWrapper.StatisticRecordRepository.GetFirstOrDefaultAsync(
                predicate: (sr) => sr.QrId == request.qrId);

            if (statRecord == null)
            {
                const string errorMsg = "Cannot find record by qrId";
                _logger.LogError($"UpdateCountStatisticRecordCommand handled with an error. {errorMsg}");
                return Result.Fail(new Error(errorMsg));
            }

            statRecord.Count++;

            _repositoryWrapper.StatisticRecordRepository.Update(statRecord);

            var resultIsSuccess = _repositoryWrapper.SaveChanges() > 0;

            if (!resultIsSuccess)
            {
                const string errorMsg = "Cannot save the data";
                _logger.LogError($"UpdateCountStatisticRecordCommand handled with an error. {errorMsg}");
                return Result.Fail(new Error(errorMsg));
            }

            return Result.Ok(Unit.Value);
        }
    }
}
