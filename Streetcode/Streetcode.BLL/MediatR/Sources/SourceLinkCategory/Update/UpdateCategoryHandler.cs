using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.MediatR.Streetcode.Fact.Update;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Sources.SourceLink.Update
{
    public class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand, Result<Unit>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;

        public UpdateCategoryHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
        }

        public async Task<Result<Unit>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = _mapper.Map<DAL.Entities.Sources.SourceLinkCategory>(request.Category);

            if (category is null)
            {
                return Result.Fail(new Error("Cannot convert null to Category"));
            }

            _repositoryWrapper.SourceCategoryRepository.Update(category);

            var resultIsSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;
            return resultIsSuccess ? Result.Ok(Unit.Value) : Result.Fail(new Error("Failed to update a category"));
        }
    }
}
