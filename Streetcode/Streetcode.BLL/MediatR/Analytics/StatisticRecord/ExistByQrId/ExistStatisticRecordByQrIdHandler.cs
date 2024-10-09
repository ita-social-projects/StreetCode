using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Analytics.StatisticRecord.ExistByQrId
{
    public class ExistStatisticRecordByQrIdHandler : IRequestHandler<ExistStatisticRecordByQrIdCommand, Result<bool>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repository;

        public ExistStatisticRecordByQrIdHandler(IMapper mapper, IRepositoryWrapper repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<Result<bool>> Handle(ExistStatisticRecordByQrIdCommand request, CancellationToken cancellationToken)
        {
            var statRecord = await _repository.StatisticRecordRepository
                .GetFirstOrDefaultAsync(predicate: (sr) => sr.QrId == request.qrId);

            return statRecord is null ? Result.Ok(false) : Result.Ok(true);
        }
    }
}
