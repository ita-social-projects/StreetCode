using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.MediatR.Streetcode.Fact.Create;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Sources.SourceLink.Create
{
    public class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, Result<DAL.Entities.Sources.SourceLinkCategory>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;

        public CreateCategoryHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
        }

        public async Task<Result<DAL.Entities.Sources.SourceLinkCategory>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = _mapper.Map<DAL.Entities.Sources.SourceLinkCategory>(request.Category);
            if (category.ImageId != 0)
            {
                category.Image = null;
            }

            if (category is null)
            {
                return Result.Fail(new Error("Cannot convert null to Category"));
            }

            var returned = _repositoryWrapper.SourceCategoryRepository.Create(category);
            var resultIsSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;
            return resultIsSuccess ? Result.Ok(returned) : Result.Fail(new Error("Failed to create a category"));
        }
    }
}
