using AutoMapper;
using FluentResults;
using MediatR;
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
                string errorMsg = "Invalid ImageId Value";
                _logger.LogError(request, errorMsg);
                return Result.Fail(errorMsg);
            }

            var imageExists = await _repositoryWrapper.ImageRepository.GetFirstOrDefaultAsync(i => i.Id == newNews.ImageId);
            if (imageExists == null)
            {
                string errorMsg = "Image with provided ImageId does not exist";
                _logger.LogError(request, errorMsg);
                return Result.Fail(errorMsg);
            }

            foreach (char c in newNews.URL)
            {
                if (!((c >= 'a' && c <= 'z') || char.IsDigit(c) || c == '-') || (c >= 'A' && c <= 'Z'))
                {
                    string errorMsg = "Url Is Invalid";
                    _logger.LogError(request, errorMsg);
                    return Result.Fail(errorMsg);
                }
            }

            if (newNews.CreationDate == default(DateTime))
            {
                string errorMsg = "CreationDate field is required";
                _logger.LogError(request, errorMsg);
                return Result.Fail(errorMsg);
            }

            var existingNewsByTitle = await _repositoryWrapper.NewsRepository.GetFirstOrDefaultAsync(predicate: n => n.Title == request.newNews.Title);
            if (existingNewsByTitle != null)
            {
                string errorMsg = "A news with the same title already exists.";
                _logger.LogError(request, errorMsg);
                return Result.Fail(errorMsg);
            }

            var existingNewsByText = await _repositoryWrapper.NewsRepository.GetSingleOrDefaultAsync(predicate: n => n.Text == request.newNews.Text);

            if (existingNewsByText != null)
            {
                string errorMsg = "A news with the same text already exists.";
                _logger.LogError(request, errorMsg);
                return Result.Fail(errorMsg);
            }

            var existingNewsByImageID = await _repositoryWrapper.NewsRepository.GetSingleOrDefaultAsync(predicate: n => n.ImageId == request.newNews.ImageId);

            if (existingNewsByImageID != null)
            {
                string errorMsg = "A news with the same ImageID already exists.";
                _logger.LogError(request, errorMsg);
                return Result.Fail(errorMsg);
            }

            var entity = _repositoryWrapper.NewsRepository.Create(newNews);
            var resultIsSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;

            if (resultIsSuccess)
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
