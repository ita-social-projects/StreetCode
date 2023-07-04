using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Analytics;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

using Entity = Streetcode.DAL.Entities.Analytics.StatisticRecord;

namespace Streetcode.BLL.MediatR.Analytics.StatisticRecord.Create
{
    public class CreateStatisticRecordHandler : IRequestHandler<CreateStatisticRecordCommand, Result<StatisticRecordDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IStringLocalizer<NoSharedResource>? _stringLocalizerNo;
        private readonly IStringLocalizer<CannotSaveSharedResource> _stringLocalizer;

        public CreateStatisticRecordHandler(
            IMapper mapper,
            IRepositoryWrapper repositoryWrapper,
            IStringLocalizer<CannotSaveSharedResource> stringLocalizer,
            IStringLocalizer<NoSharedResource> stringLocalizerNo)
        {
            _mapper = mapper;
            _repositoryWrapper = repositoryWrapper;
            _stringLocalizer = stringLocalizer;
            _stringLocalizerNo = stringLocalizerNo;
        }

        public async Task<Result<StatisticRecordDTO>> Handle(CreateStatisticRecordCommand request, CancellationToken cancellationToken)
        {
            var statRecord = _mapper.Map<Entity>(request.StatisticRecordDTO);

            if (statRecord == null)
            {
                return Result.Fail(new Error(_stringLocalizerNo?["NoMappedRecord"].Value));
            }

            var createdRecord = _repositoryWrapper.StatisticRecordRepository.Create(statRecord);

            if (createdRecord == null)
            {
                return Result.Fail(new Error(_stringLocalizer?["NoCreatedRecord"].Value));
            }

            var resultIsSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;

            if (!resultIsSuccess)
            {
                return Result.Fail(new Error(_stringLocalizer?["CannotSaveCreatedRecord"].Value));
            }

            var mappedCreatedRecord = _mapper.Map<StatisticRecordDTO>(createdRecord);

            if (mappedCreatedRecord == null)
            {
                return Result.Fail(new Error(_stringLocalizer?["NoMappedCreatedRecord"].Value));
            }

            return Result.Ok(mappedCreatedRecord);
        }
    }
}
