using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Sources;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Sources.SourceLink.Create;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Sources.SourceLinkCategory.Create
{
    public class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, Result<CreateSourceLinkCategoryDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;
        private readonly IStringLocalizer<CannotConvertNullSharedResource> _stringLocalizerCannot;

        public CreateCategoryHandler(
            IRepositoryWrapper repositoryWrapper,
            IMapper mapper,
            ILoggerService logger,
            IStringLocalizer<CannotConvertNullSharedResource> stringLocalizerCannot)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
            _stringLocalizerCannot = stringLocalizerCannot;
        }

        public async Task<Result<CreateSourceLinkCategoryDTO>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = _mapper.Map<DAL.Entities.Sources.SourceLinkCategory>(request.Category);
            if (category is null)
            {
                string errorMsg = _stringLocalizerCannot["CannotConvertNullToCategory"].Value;
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            _ = _repositoryWrapper.SourceCategoryRepository.Create(category);
            var resultIsSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;
            var returnCategory = _mapper.Map<CreateSourceLinkCategoryDTO>(category);
            if (resultIsSuccess)
            {
                return Result.Ok(returnCategory);
            }
            else
            {
                string errorMsg = "Failed to create category";
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }
        }
    }
}
