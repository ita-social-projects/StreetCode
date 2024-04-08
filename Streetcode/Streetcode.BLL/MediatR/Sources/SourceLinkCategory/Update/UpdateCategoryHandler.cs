using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Sources;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.Fact.Update;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Sources.SourceLink.Update
{
    public class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand, Result<UpdateSourceLinkCategoryDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;
        private readonly IStringLocalizer<CannotConvertNullSharedResource> _stringLocalizerCannotConvert;
        private readonly IStringLocalizer<FailedToUpdateSharedResource> _stringLocalizerFailedToUpdate;
        public UpdateCategoryHandler(
            IRepositoryWrapper repositoryWrapper,
            IMapper mapper,
            ILoggerService logger,
            IStringLocalizer<FailedToUpdateSharedResource> stringLocalizerFailedToUpdate,
            IStringLocalizer<CannotConvertNullSharedResource> stringLocalizerCannotConvert)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
            _stringLocalizerFailedToUpdate = stringLocalizerFailedToUpdate;
            _stringLocalizerCannotConvert = stringLocalizerCannotConvert;
        }

        public async Task<Result<UpdateSourceLinkCategoryDTO>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = _mapper.Map<DAL.Entities.Sources.SourceLinkCategory>(request.Category);
            if (category is null)
            {
                string errorMsg = _stringLocalizerCannotConvert["CannotConvertNullToCategory"].Value;
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            _repositoryWrapper.SourceCategoryRepository.Update(category);

            var resultIsSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;
            if(resultIsSuccess)
            {
                return Result.Ok(_mapper.Map<UpdateSourceLinkCategoryDTO>(category));
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
