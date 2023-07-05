using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Entity = Streetcode.DAL.Entities.Analytics.StatisticRecord;

namespace Streetcode.BLL.MediatR.Analytics.StatisticRecord.Delete
{
    public class DeleteStatisticRecordHandler : IRequestHandler<DeleteStatisticRecordCommand, Result<Unit>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;
        private readonly IStringLocalizer<FailedToDeleteSharedResource> _stringLocalizerFailedToDelete;

        public DeleteStatisticRecordHandler(
            IMapper mapper,
            IRepositoryWrapper repositoryWrapper,
            IStringLocalizer<FailedToDeleteSharedResource> stringLocalizerFailedToDelete,
            IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind)
        {
            _mapper = mapper;
            _repositoryWrapper = repositoryWrapper;
            _stringLocalizerFailedToDelete = stringLocalizerFailedToDelete;
            _stringLocalizerCannotFind = stringLocalizerCannotFind;
        }

        public async Task<Result<Unit>> Handle(DeleteStatisticRecordCommand request, CancellationToken cancellationToken)
        {
            var statRecord = await _repositoryWrapper.StatisticRecordRepository
                .GetFirstOrDefaultAsync(predicate: (sr) => sr.QrId == request.qrId);

            if (statRecord is null)
            {
                return Result.Fail(new Error(_stringLocalizerCannotFind["CannotFindRecordForQrId"].Value));
            }

            _repositoryWrapper.StatisticRecordRepository.Delete(statRecord);

            var resultIsSuccess = _repositoryWrapper.SaveChanges() > 0;

            return resultIsSuccess ? Result.Ok(Unit.Value) : Result.Fail(new Error(_stringLocalizerFailedToDelete["FailedToDeleteTheRecord"].Value));
        }
    }
}
