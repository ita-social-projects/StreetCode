using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Event.Delete
{
    public class DeleteEventHandler : IRequestHandler<DeleteEventCommand, Result<int>>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;
        private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;
        private readonly IStringLocalizer<FailedToDeleteSharedResource> _stringLocalizerFailedToDelete;
        public DeleteEventHandler(
            IRepositoryWrapper repositoryWrapper,
            ILoggerService logger,
            IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind,
            IStringLocalizer<FailedToDeleteSharedResource> stringLocalizerFailedToDelete)
        {
            _repositoryWrapper = repositoryWrapper;
            _logger = logger;
            _stringLocalizerCannotFind = stringLocalizerCannotFind;
            _stringLocalizerFailedToDelete = stringLocalizerFailedToDelete;
        }

        public async Task<Result<int>> Handle(DeleteEventCommand request, CancellationToken cancellationToken)
        {
            var eventToDelete = await _repositoryWrapper.EventRepository.GetFirstOrDefaultAsync(x => x.Id == request.id);

            if (eventToDelete is null)
            {
                var exMessage = _stringLocalizerCannotFind["CannotFindEventWithCorrespondingId", request.id].Value;
                _logger.LogError(request, exMessage);
                return Result.Fail(exMessage);
            }

            _repositoryWrapper.EventRepository.Delete(eventToDelete);
            var resultIsSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;

            if (resultIsSuccess)
            {
                return Result.Ok(request.id);
            }
            else
            {
                var finalExMessage = _stringLocalizerFailedToDelete["FailedToDeleteEvent"].Value;
                _logger.LogError(request, finalExMessage);
                return Result.Fail(new Error(finalExMessage));
            }
        }
    }
}