﻿using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.Interfaces.Logging;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.MediatR.Streetcode.Fact.Create;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Sources.SourceLink.Create
{
    public class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, Result<DAL.Entities.Sources.SourceLinkCategory>>
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

        public async Task<Result<DAL.Entities.Sources.SourceLinkCategory>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = _mapper.Map<DAL.Entities.Sources.SourceLinkCategory>(request.Category);
            if (category.ImageId != 0)
            {
                category.Image = null;
            }

            if (category is null)
            {
                string errorMsg = _stringLocalizerCannot["CannotConvertNullToCategory"].Value;
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            var returned = _repositoryWrapper.SourceCategoryRepository.Create(category);
            var resultIsSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;
            if(resultIsSuccess)
            {
                return Result.Ok(returned);
            }
            else
            {
                string errorMsg = _stringLocalizerFailed["FailedToCreateCategory"].Value;
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }
        }
    }
}
