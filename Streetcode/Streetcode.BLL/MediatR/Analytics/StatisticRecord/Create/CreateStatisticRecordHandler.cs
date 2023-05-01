using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Analytics;
using Streetcode.DAL.Repositories.Interfaces.Base;

using Entity = Streetcode.DAL.Entities.Analytics.StatisticRecord;

namespace Streetcode.BLL.MediatR.Analytics.StatisticRecord.Create
{
    public class CreateStatisticRecordHandler : IRequestHandler<CreateStatisticRecordCommand, Result<StatisticRecordDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;

        public CreateStatisticRecordHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper)
        {
            _mapper = mapper;
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<Result<StatisticRecordDTO>> Handle(CreateStatisticRecordCommand request, CancellationToken cancellationToken)
        {
            var statRecord = _mapper.Map<Entity>(request.StatisticRecordDTO);

            if (statRecord == null)
            {
                return Result.Fail(new Error("Mapped record is null"));
            }

            var createdRecord = _repositoryWrapper.StatisticRecordRepository.Create(statRecord);

            if (createdRecord == null)
            {
                return Result.Fail(new Error("Created record is null"));
            }

            var resultIsSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;

            if (!resultIsSuccess)
            {
                return Result.Fail(new Error("Cannot save created record"));
            }

            var mappedCreatedRecord = _mapper.Map<StatisticRecordDTO>(createdRecord);

            if (mappedCreatedRecord == null)
            {
                return Result.Fail(new Error("Mapped created record is null"));
            }

            return Result.Ok(mappedCreatedRecord);
        }
    }
}
