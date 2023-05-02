using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Analytics.StatisticRecord.UpdateCount
{
    public class UpdateCountStatisticRecordHandler : IRequestHandler<UpdateCountStatisticRecordCommand, Result<Unit>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;

        public UpdateCountStatisticRecordHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper)
        {
            _mapper = mapper;
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<Result<Unit>> Handle(UpdateCountStatisticRecordCommand request, CancellationToken cancellationToken)
        {
            var statRecord = await _repositoryWrapper.StatisticRecordRepository.GetFirstOrDefaultAsync(
                predicate: (sr) => sr.QrId == request.qrId);

            if (statRecord == null)
            {
                return Result.Fail(new Error("Cannot find record by qrId"));
            }

            statRecord.Count++;

            _repositoryWrapper.StatisticRecordRepository.Update(statRecord);

            var resultIsSuccess = _repositoryWrapper.SaveChanges() > 0;

            if (!resultIsSuccess)
            {
                return Result.Fail(new Error("Cannot save the data"));
            }

            return Result.Ok(Unit.Value);
        }
    }
}
