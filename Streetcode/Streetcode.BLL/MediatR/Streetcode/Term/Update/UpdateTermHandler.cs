using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.Interfaces.Logging;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Term.Update
{
    public class UpdateTermHandler : IRequestHandler<UpdateTermCommand, Result<Unit>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repository;
        private readonly ILoggerService _logger;
        private readonly IStringLocalizer<CannotConvertNullSharedResource> _stringLocalizerCannotConvertNull;
        private readonly IStringLocalizer<FailedToUpdateSharedResource> _stringLocalizerFailedToUpdate;

        public UpdateTermHandler(IMapper mapper, IRepositoryWrapper repository, ILoggerService logger, IStringLocalizer<FailedToUpdateSharedResource> stringLocalizerFailedToUpdate, IStringLocalizer<CannotConvertNullSharedResource> stringLocalizerCannotConvertNull)
        {
            _mapper = mapper;
            _repository = repository;
            _logger = logger;
            _stringLocalizerFailedToUpdate = stringLocalizerFailedToUpdate;
            _stringLocalizerCannotConvertNull = stringLocalizerCannotConvertNull;
        }

        public async Task<Result<Unit>> Handle(UpdateTermCommand request, CancellationToken cancellationToken)
        {
            var term = _mapper.Map<DAL.Entities.Streetcode.TextContent.Term>(request.Term);
            if (term is null)
            {
                const string errorMsg = _stringLocalizerCannotConvertNull["CannotConvertNullToTerm"].Value;
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            _repository.TermRepository.Update(term);

            var resultIsSuccess = await _repository.SaveChangesAsync() > 0;
            if (resultIsSuccess)
            {
                return Result.Ok(Unit.Value);
            }
            else
            {
                const string errorMsg = _stringLocalizerFailedToUpdate["FailedToUpdateTerm"].Value;
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }
        }
    }
}
