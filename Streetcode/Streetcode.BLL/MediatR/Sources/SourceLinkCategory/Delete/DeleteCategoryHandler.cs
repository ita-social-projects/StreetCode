using FluentResults;
using MediatR;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Sources.SourceLink.Delete
{
    public class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand, Result<Unit>>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;

        public DeleteCategoryHandler(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<Result<Unit>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _repositoryWrapper.SourceCategoryRepository.GetFirstOrDefaultAsync(f => f.Id == request.Id);

            if (category is null)
            {
                return Result.Fail(new Error($"Cannot find a category with corresponding categoryId: {request.Id}"));
            }

            _repositoryWrapper.SourceCategoryRepository.Delete(category);

            var resultIsSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;
            return resultIsSuccess ? Result.Ok(Unit.Value) : Result.Fail(new Error("Failed to delete a category"));
        }
    }
}
