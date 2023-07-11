using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Entity = Streetcode.DAL.Entities.Analytics.StatisticRecord;

namespace Streetcode.BLL.MediatR.Analytics.StatisticRecord.Delete
{
    public class DeleteStatisticRecordHandler : IRequestHandler<DeleteStatisticRecordCommand, Result<Unit>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;

        public DeleteStatisticRecordHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper, ILoggerService logger)
        {
            _mapper = mapper;
            _repositoryWrapper = repositoryWrapper;
            _logger = logger;
        }

        public async Task<Result<Unit>> Handle(DeleteStatisticRecordCommand request, CancellationToken cancellationToken)
        {
            var statRecord = await _repositoryWrapper.StatisticRecordRepository
                .GetFirstOrDefaultAsync(predicate: (sr) => sr.QrId == request.qrId);

            if (statRecord is null)
            {
                const string errorMsg = "Cannot find record for qrId";
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            _repositoryWrapper.StatisticRecordRepository.Delete(statRecord);

            var resultIsSuccess = _repositoryWrapper.SaveChanges() > 0;

            if(resultIsSuccess)
            {
                return Result.Ok(Unit.Value);
            }
            else
            {
                const string errorMsg = "Cannot delete the record";
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }
        }
    }
}
