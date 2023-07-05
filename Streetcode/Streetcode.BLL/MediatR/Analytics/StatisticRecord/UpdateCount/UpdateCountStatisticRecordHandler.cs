using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Analytics.StatisticRecord.UpdateCount
{
    public class UpdateCountStatisticRecordHandler : IRequestHandler<UpdateCountStatisticRecordCommand, Result<Unit>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;
        private readonly IStringLocalizer<CannotSaveSharedResource> _stringLocalizerCannotSave;

        public UpdateCountStatisticRecordHandler(
            IMapper mapper,
            IRepositoryWrapper repositoryWrapper,
            IStringLocalizer<CannotSaveSharedResource> stringLocalizerCannotSave,
            IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind)
        {
            _mapper = mapper;
            _repositoryWrapper = repositoryWrapper;
            _stringLocalizerCannotSave = stringLocalizerCannotSave;
            _stringLocalizerCannotFind = stringLocalizerCannotFind;
        }

        public async Task<Result<Unit>> Handle(UpdateCountStatisticRecordCommand request, CancellationToken cancellationToken)
        {
            var statRecord = await _repositoryWrapper.StatisticRecordRepository.GetFirstOrDefaultAsync(
                predicate: (sr) => sr.QrId == request.qrId);

            if (statRecord == null)
            {
                return Result.Fail(new Error(_stringLocalizerCannotFind["CannotFindRecordWithQrId"].Value));
            }

            statRecord.Count++;

            _repositoryWrapper.StatisticRecordRepository.Update(statRecord);

            var resultIsSuccess = _repositoryWrapper.SaveChanges() > 0;

            if (!resultIsSuccess)
            {
                return Result.Fail(new Error(_stringLocalizerCannotSave["CannotSaveTheData"].Value));
            }

            return Result.Ok(Unit.Value);
        }
    }
}
