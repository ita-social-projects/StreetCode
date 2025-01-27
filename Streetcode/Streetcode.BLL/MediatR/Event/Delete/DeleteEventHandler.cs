using FluentResults;
using MediatR;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Event.Delete
{
    public class DeleteEventHandler : IRequestHandler<DeleteEventCommand, Result<int>>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;
        public DeleteEventHandler(IRepositoryWrapper repositoryWrapper, ILoggerService logger)
        {
            _repositoryWrapper = repositoryWrapper;
            _logger = logger;
        }

        public async Task<Result<int>> Handle(DeleteEventCommand request, CancellationToken cancellationToken)
        {
            var eventToDelete = await _repositoryWrapper.EventRepository.GetFirstOrDefaultAsync(x => x.Id == request.id);

            if (eventToDelete is null)
            {
                string exMessage = $"No event found by entered Id - {request.id}";
                _logger.LogError(request, exMessage);
                return Result.Fail(exMessage);
            }

            try
            {
                _repositoryWrapper.EventRepository.Delete(eventToDelete);
                await _repositoryWrapper.SaveChangesAsync();
                return Result.Ok(request.id);
            }
            catch (Exception ex)
            {
                _logger.LogError(request, ex.Message);
                return Result.Fail(ex.Message);
            }
        }
    }
}
