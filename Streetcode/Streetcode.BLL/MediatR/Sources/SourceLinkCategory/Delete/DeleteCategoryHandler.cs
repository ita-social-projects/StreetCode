using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Sources.SourceLink.Delete
{
    public class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand, Result<Unit>>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;
        private readonly IStringLocalizer<FailedToDeleteSharedResource> _stringLocalizerFailedToDelete;

        public DeleteCategoryHandler(
            IRepositoryWrapper repositoryWrapper,
            IStringLocalizer<FailedToDeleteSharedResource> stringLocalizerFailedToDelete,
            IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind)
        {
            _repositoryWrapper = repositoryWrapper;
            _stringLocalizerFailedToDelete = stringLocalizerFailedToDelete;
            _stringLocalizerCannotFind = stringLocalizerCannotFind;
        }

        public async Task<Result<Unit>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _repositoryWrapper.SourceCategoryRepository.GetFirstOrDefaultAsync(f => f.Id == request.Id);

            if (category is null)
            {
                return Result.Fail(new Error(_stringLocalizerCannotFind["CannotFindCategoryWithCorrespondingCategoryId", request.Id].Value));
            }

            _repositoryWrapper.SourceCategoryRepository.Delete(category);

            var resultIsSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;
            return resultIsSuccess ? Result.Ok(Unit.Value) : Result.Fail(new Error(_stringLocalizerFailedToDelete["FailedToDeleteCategory"].Value));
        }
    }
}
