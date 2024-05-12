using FluentResults;
using MediatR;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.AdditionalContent.Tag.Delete
{
    public class DeleteTagHandler : IRequestHandler<DeleteTagCommand, Result<int>>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;

        public DeleteTagHandler(IRepositoryWrapper repository, ILoggerService logger)
        {
            _repositoryWrapper = repository;
            _logger = logger;
        }

        public async Task<Result<int>> Handle(DeleteTagCommand request, CancellationToken cancellationToken)
        {
            var tagToDelete =
                await _repositoryWrapper.TagRepository.GetFirstOrDefaultAsync(x => x.Id == request.id);
            if (tagToDelete is null)
            {
                string exMessage = $"No tag found by entered Id - {request.id}";
                _logger.LogError(request, exMessage);
                return Result.Fail(exMessage);
            }

            try
            {
                _repositoryWrapper.TagRepository.Delete(tagToDelete);
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
