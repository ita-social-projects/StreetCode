using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.News;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.News;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Newss.Create
{
    public class CreateNewsHandler : IRequestHandler<CreateNewsCommand, Result<NewsDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;
        private readonly IStringLocalizer<CannotConvertNullSharedResource> _stringLocalizerCannot;
        private readonly IStringLocalizer<FailedToCreateSharedResource> _stringLocalizerFailed;
        public CreateNewsHandler(
            IMapper mapper,
            IRepositoryWrapper repositoryWrapper,
            ILoggerService logger,
            IStringLocalizer<FailedToCreateSharedResource> stringLocalizerFailed,
            IStringLocalizer<CannotConvertNullSharedResource> stringLocalizerCannot)
        {
            _mapper = mapper;
            _repositoryWrapper = repositoryWrapper;
            _logger = logger;
            _stringLocalizerFailed = stringLocalizerFailed;
            _stringLocalizerCannot = stringLocalizerCannot;
        }

        public async Task<Result<NewsDTO>> Handle(CreateNewsCommand request, CancellationToken cancellationToken)
        {
            var newNews = _mapper.Map<News>(request.newNews);
            if (newNews is null)
            {
                string errorMsg = _stringLocalizerCannot["CannotConvertNullToNews"].Value;
                _logger.LogError(request, errorMsg);
                return Result.Fail(errorMsg);
            }

            if (newNews.ImageId == 0)
            {
                string errorMsg = _stringLocalizerFailed["Invalid imageId value"].Value;
                _logger.LogError(request, errorMsg);
                return Result.Fail(errorMsg);
            }

            var entity = _repositoryWrapper.NewsRepository.Create(newNews);
            var resultIsSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;
            if(resultIsSuccess)
            {
                return Result.Ok(_mapper.Map<NewsDTO>(entity));
            }
            else
            {
                string errorMsg = _stringLocalizerFailed["FailedToCreateNews"].Value;
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }
        }
    }
}
