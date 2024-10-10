using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Analytics;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

using Entity = Streetcode.DAL.Entities.Analytics.StatisticRecord;

namespace Streetcode.BLL.MediatR.Analytics.StatisticRecord.Create
{
    public class CreateStatisticRecordHandler : IRequestHandler<CreateStatisticRecordCommand, Result<StatisticRecordResponseDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;
        private readonly IStringLocalizer<NoSharedResource> _stringLocalizerNo;
        private readonly IStringLocalizer<CannotSaveSharedResource> _stringLocalizer;

        public CreateStatisticRecordHandler(
            IMapper mapper,
            IRepositoryWrapper repositoryWrapper,
            ILoggerService logger,
            IStringLocalizer<CannotSaveSharedResource> stringLocalizer,
            IStringLocalizer<NoSharedResource> stringLocalizerNo)
        {
            _mapper = mapper;
            _repositoryWrapper = repositoryWrapper;
            _logger = logger;
            _stringLocalizer = stringLocalizer;
            _stringLocalizerNo = stringLocalizerNo;
        }

        public async Task<Result<StatisticRecordResponseDTO>> Handle(CreateStatisticRecordCommand request, CancellationToken cancellationToken)
        {
            var statRecord = _mapper.Map<Entity>(request.StatisticRecordDTO);

            if (statRecord == null)
            {
                string errorMsg = _stringLocalizerNo["NoMappedRecord"].Value;
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            var createdRecord = _repositoryWrapper.StatisticRecordRepository.Create(statRecord);

            if (createdRecord == null)
            {
                string errorMsg = _stringLocalizer["NoCreatedRecord"].Value;
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            var resultIsSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;

            if (!resultIsSuccess)
            {
                string errorMsg = _stringLocalizer["CannotSaveCreatedRecord"].Value;
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            var mappedCreatedRecord = _mapper.Map<StatisticRecordResponseDTO>(createdRecord);

            if (mappedCreatedRecord == null)
            {
                string errorMsg = _stringLocalizer["NoMappedCreatedRecord"].Value;
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            return Result.Ok(mappedCreatedRecord);
        }
    }
}
