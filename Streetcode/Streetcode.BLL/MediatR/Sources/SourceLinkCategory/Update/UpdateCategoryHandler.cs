﻿using AutoMapper;
using FluentResults;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.News;
using Streetcode.BLL.DTO.Sources;
using Streetcode.BLL.DTO.Sources.Validation;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.Fact.Update;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.News;
using Streetcode.DAL.Repositories.Interfaces.Base;
using SourcesEntity = Streetcode.DAL.Entities.Sources;

namespace Streetcode.BLL.MediatR.Sources.SourceLink.Update
{
    public class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand, Result<UpdateSourceLinkCategoryDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;
        private readonly IStringLocalizer<CannotConvertNullSharedResource> _stringLocalizerCannotConvert;
        private readonly IStringLocalizer<FailedToUpdateSharedResource> _stringLocalizerFailedToUpdate;
        private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFindSharedResource;
        public UpdateCategoryHandler(
            IRepositoryWrapper repositoryWrapper,
            IMapper mapper,
            ILoggerService logger,
            IStringLocalizer<FailedToUpdateSharedResource> stringLocalizerFailedToUpdate,
            IStringLocalizer<CannotConvertNullSharedResource> stringLocalizerCannotConvert,
            IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
            _stringLocalizerFailedToUpdate = stringLocalizerFailedToUpdate;
            _stringLocalizerCannotConvert = stringLocalizerCannotConvert;
            _stringLocalizerCannotFindSharedResource = stringLocalizerCannotFind;
        }

        public async Task<Result<UpdateSourceLinkCategoryDTO>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var existingCategory = await _repositoryWrapper.SourceCategoryRepository
                .GetFirstOrDefaultAsync(a => a.Id == request.Category.Id);

            if (existingCategory is null)
            {
                string errorMsg = _stringLocalizerCannotFindSharedResource["CannotFindAnySrcCategoryByTheCorrespondingId", request.Category.Id].Value;
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            var existingTitleCategory = await _repositoryWrapper.SourceCategoryRepository
                    .GetFirstOrDefaultAsync(a => a.Title == request.Category.Title);
            if (existingTitleCategory is not null && existingTitleCategory.Id != request.Category.Id)
            {
                string errorMsg = $"Title: {request.Category.Title} already exists";
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            var validator = new SourceLinkCategoryDTOValidator();
            var validationResult = validator.Validate(request.Category);

            if (!validationResult.IsValid)
            {
                string errorMsg = validationResult.Errors.First().ErrorMessage;
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            var category = _mapper.Map<SourcesEntity.SourceLinkCategory>(request.Category);
            if (category is null)
            {
                string errorMsg = _stringLocalizerCannotConvert["CannotConvertNullToCategory"].Value;
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

            _repositoryWrapper.SourceCategoryRepository.Update(category);

            var resultIsSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;
            var returnCategory = _mapper.Map<UpdateSourceLinkCategoryDTO>(category);
            if (resultIsSuccess)
            {
                return Result.Ok(returnCategory);
            }
            else
            {
                string errorMsg = _stringLocalizerFailedToUpdate["FailedToUpdateCategory"].Value;
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }
        }
    }
}
