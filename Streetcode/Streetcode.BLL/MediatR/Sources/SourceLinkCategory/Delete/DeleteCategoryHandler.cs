using FluentResults;
using MediatR;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Sources.SourceLink.Delete
{
    public class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand, Result<Unit>>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService? _logger;

        public DeleteCategoryHandler(IRepositoryWrapper repositoryWrapper, ILoggerService? logger = null)
        {
            _repositoryWrapper = repositoryWrapper;
            _logger = logger;
        }

        public async Task<Result<Unit>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _repositoryWrapper.SourceCategoryRepository.GetFirstOrDefaultAsync(f => f.Id == request.Id);

            if (category is null)
            {
                string errorMsg = $"Cannot find a category with corresponding categoryId: {request.Id}";
                _logger?.LogError("DeleteCategoryCommand handled with an error");
                _logger?.LogError(errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            _repositoryWrapper.SourceCategoryRepository.Delete(category);

            var resultIsSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;
            if(resultIsSuccess)
            {
                _logger?.LogInformation($"DeleteCategoryCommand handled successfully");
                return Result.Ok(Unit.Value);
            }
            else
            {
                string errorMsg = "Failed to delete a category";
                _logger?.LogError("DeleteCategoryCommand handled with an error");
                _logger?.LogError(errorMsg);
                return Result.Fail(new Error(errorMsg));
            }
        }
    }
}
