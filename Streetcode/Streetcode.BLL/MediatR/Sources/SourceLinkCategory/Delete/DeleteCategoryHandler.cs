using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Sources.SourceLink.Delete
{
    public class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand, Result<Unit>>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;
        private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;
        private readonly IStringLocalizer<FailedToDeleteSharedResource> _stringLocalizerFailedToDelete;

        public DeleteCategoryHandler(
            IRepositoryWrapper repositoryWrapper,
            ILoggerService logger,
            IStringLocalizer<FailedToDeleteSharedResource> stringLocalizerFailedToDelete,
            IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind)
        {
            _repositoryWrapper = repositoryWrapper;
            _logger = logger;
            _stringLocalizerFailedToDelete = stringLocalizerFailedToDelete;
            _stringLocalizerCannotFind = stringLocalizerCannotFind;
        }

        public async Task<Result<Unit>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _repositoryWrapper.SourceCategoryRepository.GetFirstOrDefaultAsync(f => f.Id == request.Id);

            if (category is null)
            {
                string errorMsg = _stringLocalizerCannotFind["CannotFindCategoryWithCorrespondingCategoryId", request.Id].Value;
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            _repositoryWrapper.SourceCategoryRepository.Delete(category);

            var resultIsSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;
            if(resultIsSuccess)
            {
                return Result.Ok(Unit.Value);
            }
            else
            {
                string errorMsg = _stringLocalizerFailedToDelete["FailedToDeleteCategory"].Value;
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }
        }
    }
}
