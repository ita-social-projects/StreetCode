using System.Linq.Expressions;
using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.News;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Services.EntityAccessManager;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.News;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Newss.GetNewsAndLinksByUrl
{
    public class GetNewsAndLinksByUrlHandler : IRequestHandler<GetNewsAndLinksByUrlQuery, Result<NewsDTOWithURLs>>
    {
        private const int MinNumberOfNews = 10;
        private const int NumberOfMonthsBackForRelevantNews = 6;
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

        public async Task<Result<NewsDTOWithURLs>> Handle(GetNewsAndLinksByUrlQuery request, CancellationToken cancellationToken)
        {
            string url = request.Url;
            Expression<Func<News, bool>>? basePredicate = nw => nw.URL == url;
            var predicate = basePredicate.ExtendWithAccessPredicate(new NewsAccessManager(), request.UserRole);

            var newsDTO = _mapper.Map<NewsDTO>(await _repositoryWrapper.NewsRepository.GetFirstOrDefaultAsync(
                predicate: predicate,
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

            DateTime sixMonthsAgo = DateTime.UtcNow.AddMonths(-NumberOfMonthsBackForRelevantNews);
            basePredicate = nw => nw.CreationDate > sixMonthsAgo;
            predicate = basePredicate.ExtendWithAccessPredicate(new NewsAccessManager(), request.UserRole);

            var newsCount = await _repositoryWrapper.NewsRepository.FindAll(predicate: predicate).CountAsync(cancellationToken);
            List<News> news;
            if (newsCount >= MinNumberOfNews)
            {
                news = (await _repositoryWrapper.NewsRepository.GetAllAsync(predicate: predicate)).OrderByDescending(nw => nw.CreationDate).ToList();
            }
            else
            {
                news = _repositoryWrapper.NewsRepository.GetAllPaginated(1, MinNumberOfNews, predicate: predicate, descendingSortKeySelector: nw => nw.CreationDate).Entities.ToList();
            }

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

            RandomNewsDTO? randomNewsDTO = null;
            if (news.Count > 1)
            {
                Random rnd = new();
                var randomIndex = rnd.Next(news.Count);
                while (randomIndex == newsIndex)
                {
                    randomIndex = rnd.Next(news.Count);
                }

                randomNewsDTO = new RandomNewsDTO();
                randomNewsDTO.Title = news[randomIndex].Title;
                randomNewsDTO.RandomNewsUrl = news[randomIndex].URL;
            }

            var newsDTOWithUrls = new NewsDTOWithURLs();
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