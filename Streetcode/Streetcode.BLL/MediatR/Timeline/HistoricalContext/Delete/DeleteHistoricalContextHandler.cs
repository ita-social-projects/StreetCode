using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Timeline.HistoricalContext.Delete
{
    public class DeleteHistoricalContextHandler : IRequestHandler<DeleteHistoricalContextCommand, Result<int>>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;
        private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizer;

        public DeleteHistoricalContextHandler(IRepositoryWrapper repository, ILoggerService logger, IStringLocalizer<CannotFindSharedResource> stringLocalizer)
        {
            _repositoryWrapper = repository;
            _logger = logger;
            _stringLocalizer = stringLocalizer;
        }

        public async Task<Result<int>> Handle(DeleteHistoricalContextCommand request, CancellationToken cancellationToken)
        {
            var contextToDelete =
                await _repositoryWrapper.HistoricalContextRepository.GetFirstOrDefaultAsync(x => x.Id == request.contextId);
            if (contextToDelete is null)
            {
                string exMessage = _stringLocalizer["CannotFindHistoricalContextWithCorrespondingId", request.contextId];
                _logger.LogError(request, exMessage);
                return Result.Fail(exMessage);
            }

            try
            {
                _repositoryWrapper.HistoricalContextRepository.Delete(contextToDelete);
                await _repositoryWrapper.SaveChangesAsync();
                return Result.Ok(request.contextId);
            }
            catch(Exception ex)
            {
                _logger.LogError(request, ex.Message);
                return Result.Fail(ex.Message);
            }
        }
    }
}