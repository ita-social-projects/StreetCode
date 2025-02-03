using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.News;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Newss.GetNewsAndLinksByUrl
{
    public class GetNewsAndLinksByUrlHandler : IRequestHandler<GetNewsAndLinksByUrlQuery, Result<NewsDtoWithUrls>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IBlobService _blobService;
        private readonly ILoggerService _logger;
        private readonly IStringLocalizer<NoSharedResource> _stringLocalizerNo;
        public GetNewsAndLinksByUrlHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper, IBlobService blobService, ILoggerService logger, IStringLocalizer<NoSharedResource> stringLocalizerNo)
        {
            _mapper = mapper;
            _repositoryWrapper = repositoryWrapper;
            _blobService = blobService;
            _logger = logger;
            _stringLocalizerNo = stringLocalizerNo;
        }

        public async Task<Result<NewsDtoWithUrls>> Handle(GetNewsAndLinksByUrlQuery request, CancellationToken cancellationToken)
        {
            string url = request.url;
            var newsDTO = _mapper.Map<NewsDto>(await _repositoryWrapper.NewsRepository.GetFirstOrDefaultAsync(
                predicate: sc => sc.URL == url,
                include: scl => scl
                    .Include(sc => sc.Image!)));

            if (newsDTO is null)
            {
                string errorMsg = _stringLocalizerNo["NoNewsByEnteredUrl", url].Value;
                _logger.LogError(request, errorMsg);
                return Result.Ok();
            }

            if (newsDTO.Image is not null)
            {
                newsDTO.Image.Base64 = _blobService.FindFileInStorageAsBase64(newsDTO.Image.BlobName);
            }

            var news = (await _repositoryWrapper.NewsRepository.GetAllAsync()).ToList();
            var newsIndex = news.FindIndex(x => x.Id == newsDTO.Id);
            string? prevNewsLink = null;
            string? nextNewsLink = null;

            if(newsIndex != 0)
            {
                prevNewsLink = news[newsIndex - 1].URL;
            }

            if(newsIndex != news.Count - 1)
            {
                nextNewsLink = news[newsIndex + 1].URL;
            }

            RandomNewsDto? randomNewsDTO = null;
            if (news.Count > 1)
            {
                Random rnd = new();
                var randomIndex = rnd.Next(news.Count);
                while (randomIndex == newsIndex)
                {
                    randomIndex = rnd.Next(news.Count);
                }

                randomNewsDTO = new RandomNewsDto();
                randomNewsDTO.Title = news[randomIndex].Title;
                randomNewsDTO.RandomNewsUrl = news[randomIndex].URL;
            }

            var newsDTOWithUrls = new NewsDtoWithUrls();
            newsDTOWithUrls.RandomNews = randomNewsDTO;
            newsDTOWithUrls.News = newsDTO;
            newsDTOWithUrls.NextNewsUrl = nextNewsLink!;
            newsDTOWithUrls.PrevNewsUrl = prevNewsLink!;

            if (newsDTOWithUrls is null)
            {
                string errorMsg = _stringLocalizerNo["NoNewsByEnteredUrl", url].Value;
                _logger.LogError(request, errorMsg);
            }

            return Result.Ok(newsDTOWithUrls!);
        }
    }
}