using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.Fact.Update;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Sources.SourceLink.Update
{
    public class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand, Result<Unit>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService? _logger;
        public UpdateCategoryHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService? logger = null)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<Unit>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = _mapper.Map<DAL.Entities.Sources.SourceLinkCategory>(request.Category);
            if (category is null)
            {
                const string errorMsg = "Cannot convert null to Category";
                _logger?.LogError("UpdateCategoryCommand handled with an error");
                _logger?.LogError(errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            _repositoryWrapper.SourceCategoryRepository.Update(category);

            var resultIsSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;
            if(resultIsSuccess)
            {
                _logger?.LogInformation($"UpdateCategoryCommand handled successfully");
                return Result.Ok(Unit.Value);
            }
            else
            {
                const string errorMsg = "Failed to update a category";
                _logger?.LogError("UpdateCategoryCommand handled with an error");
                _logger?.LogError(errorMsg);
                return Result.Fail(new Error(errorMsg));
            }
        }
    }
}
