using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Analytics;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Repositories.Interfaces.Base;

using Entity = Streetcode.DAL.Entities.Analytics.StatisticRecord;

namespace Streetcode.BLL.MediatR.Analytics.StatisticRecord.Create
{
    public class CreateStatisticRecordHandler : IRequestHandler<CreateStatisticRecordCommand, Result<StatisticRecordDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;

        public CreateStatisticRecordHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper, ILoggerService logger)
        {
            _mapper = mapper;
            _repositoryWrapper = repositoryWrapper;
            _logger = logger;
        }

        public async Task<Result<StatisticRecordDTO>> Handle(CreateStatisticRecordCommand request, CancellationToken cancellationToken)
        {
            var statRecord = _mapper.Map<Entity>(request.StatisticRecordDTO);

            if (statRecord == null)
            {
                const string errorMsg = "Mapped record is null";
                _logger.LogError($"CreateStatisticRecordCommand handled with an error. {errorMsg}");
                return Result.Fail(new Error(errorMsg));
            }

            var createdRecord = _repositoryWrapper.StatisticRecordRepository.Create(statRecord);

            if (createdRecord == null)
            {
                const string errorMsg = "Created record is null";
                _logger.LogError($"CreateStatisticRecordCommand handled with an error. {errorMsg}");
                return Result.Fail(new Error(errorMsg));
            }

            var resultIsSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;

            if (!resultIsSuccess)
            {
                const string errorMsg = "Cannot save created record";
                _logger.LogError($"CreateStatisticRecordCommand handled with an error. {errorMsg}");
                return Result.Fail(new Error(errorMsg));
            }

            var mappedCreatedRecord = _mapper.Map<StatisticRecordDTO>(createdRecord);

            if (mappedCreatedRecord == null)
            {
                const string errorMsg = "Mapped created record is null";
                _logger?.LogError($"CreateStatisticRecordCommand handled with an error. {errorMsg}");
                return Result.Fail(new Error(errorMsg));
            }

            return Result.Ok(mappedCreatedRecord);
        }
    }
}
