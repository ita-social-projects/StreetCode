using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.Interfaces.Logging;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.MediatR.Streetcode.Fact.Create;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.BLL.DTO.Sources;
using Streetcode.BLL.DTO.Sources.Validation;

namespace Streetcode.BLL.MediatR.Sources.SourceLink.Create
{
    public class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, Result<CreateSourceLinkCategoryDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;
        private readonly IStringLocalizer<CannotConvertNullSharedResource> _stringLocalizerCannot;
        private readonly IStringLocalizer<FailedToCreateSharedResource> _stringLocalizerFailed;

        public CreateCategoryHandler(
            IRepositoryWrapper repositoryWrapper,
            IMapper mapper,
            ILoggerService logger,
            IStringLocalizer<FailedToCreateSharedResource> stringLocalizerFailed,
            IStringLocalizer<CannotConvertNullSharedResource> stringLocalizerCannot)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
            _stringLocalizerFailed = stringLocalizerFailed;
            _stringLocalizerCannot = stringLocalizerCannot;
        }

        public async Task<Result<CreateSourceLinkCategoryDTO>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var validator = new SourceLinkCategoryDTOValidator();
            var validationResult = validator.Validate(request.Category);

            var existingTitleCategory = await _repositoryWrapper.SourceCategoryRepository
                        .GetFirstOrDefaultAsync(a => a.Title == request.Category.Title);
            if(existingTitleCategory is not null)
            {
                string errorMsg = $"Title: {request.Category.Title} already exists";
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            if (!validationResult.IsValid)
            {
                string errorMsg = validationResult.Errors.First().ErrorMessage;
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            var category = _mapper.Map<DAL.Entities.Sources.SourceLinkCategory>(request.Category);
            if (category is null)
            {
                string errorMsg = _stringLocalizerCannot["CannotConvertNullToCategory"].Value;
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            var existingImage = await _repositoryWrapper.ImageRepository
                .GetFirstOrDefaultAsync(a => a.Id == request.Category.ImageId);

            if (existingImage is null)
            {
                string errorMsg = $"Cannot find an image with corresponding id: {request.Category.ImageId}";
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            var returned = _repositoryWrapper.SourceCategoryRepository.Create(category);
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
