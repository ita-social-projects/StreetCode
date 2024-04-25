using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.News;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.News;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Newss.Update
{
    public class UpdateNewsHandler : IRequestHandler<UpdateNewsCommand, Result<NewsDTO>>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly IBlobService _blobSevice;
        private readonly ILoggerService _logger;
        private readonly IStringLocalizer<CannotConvertNullSharedResource> _stringLocalizerCannotConvertNull;
        private readonly IStringLocalizer<FailedToUpdateSharedResource> _stringLocalizerFailedToUpdate;
        public UpdateNewsHandler(
            IRepositoryWrapper repositoryWrapper,
            IMapper mapper,
            IBlobService blobService,
            ILoggerService logger,
            IStringLocalizer<FailedToUpdateSharedResource> stringLocalizerFailedToUpdate,
            IStringLocalizer<CannotConvertNullSharedResource> stringLocalizerCannotConvertNull)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _blobSevice = blobService;
            _logger = logger;
            _stringLocalizerFailedToUpdate = stringLocalizerFailedToUpdate;
            _stringLocalizerCannotConvertNull = stringLocalizerCannotConvertNull;
        }

        public async Task<Result<NewsDTO>> Handle(UpdateNewsCommand request, CancellationToken cancellationToken)
        {
            var news = _mapper.Map<News>(request.news);
            if (news is null)
            {
                string errorMsg = _stringLocalizerCannotConvertNull["CannotConvertNullToNews"].Value;
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            if (news.ImageId == 0)
            {
                string errorMsg = _stringLocalizerFailedToUpdate["Invalid imageId value"].Value;
                _logger.LogError(request, errorMsg);
                return Result.Fail(errorMsg);
            }

            var existingNewsByTitle = await _repositoryWrapper.NewsRepository.GetFirstOrDefaultAsync(predicate: n => n.Title == request.news.Title);
            if (existingNewsByTitle != null)
            {
                string errorMsg = "A news with the same title already exists.";
                _logger.LogError(request, errorMsg);
                return Result.Fail(errorMsg);
            }

            var existingNewsByText = await _repositoryWrapper.NewsRepository.GetSingleOrDefaultAsync(predicate: n => n.Text == request.news.Text);
            if (existingNewsByText != null)
            {
                string errorMsg = "A news with the same text already exists.";
                _logger.LogError(request, errorMsg);
                return Result.Fail(errorMsg);
            }

            var response = _mapper.Map<NewsDTO>(news);

            if (news.Image is not null)
            {
                response.Image.Base64 = _blobSevice.FindFileInStorageAsBase64(response.Image.BlobName);
            }
            else
            {
                var img = await _repositoryWrapper.ImageRepository.GetFirstOrDefaultAsync(x => x.Id == response.ImageId);
                if (img != null)
                {
                    _repositoryWrapper.ImageRepository.Delete(img);
                }
            }

            _repositoryWrapper.NewsRepository.Update(news);
            var resultIsSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;

            if(resultIsSuccess)
            {
                return Result.Ok(response);
            }
            else
            {
                string errorMsg = _stringLocalizerFailedToUpdate["FailedToUpdateNews"].Value;
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }
        }
    }
}
