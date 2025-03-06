using System.Linq.Expressions;
using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.Services.EntityAccessManager;
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
            Expression<Func<DAL.Entities.Analytics.StatisticRecord, bool>>? basePredicate = sr => sr.QrId == request.QrId;
            var predicate = basePredicate.ExtendWithAccessPredicate(new StreetcodeAccessManager(), request.UserRole, sr => sr.Streetcode);

            var statRecord = await _repository.StatisticRecordRepository
                .GetFirstOrDefaultAsync(predicate: predicate);

            return statRecord is null ? Result.Ok(false) : Result.Ok(true);
        }
    }
}
