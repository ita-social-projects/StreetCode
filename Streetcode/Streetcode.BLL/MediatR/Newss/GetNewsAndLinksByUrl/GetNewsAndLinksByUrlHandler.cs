using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.News;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.DAL.Entities.News;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Microsoft.EntityFrameworkCore;

namespace Streetcode.BLL.MediatR.Newss.GetNewsAndLinksByUrl
{
    public class GetNewsAndLinksByUrlHandler : IRequestHandler<GetNewsAndLinksByUrlQuery, Result<NewsDTOWithURLs>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IBlobService _blobService;
        public GetNewsAndLinksByUrlHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper, IBlobService blobService)
        {
            _mapper = mapper;
            _repositoryWrapper = repositoryWrapper;
            _blobService = blobService;
        }

        public async Task<Result<NewsDTOWithURLs>> Handle(GetNewsAndLinksByUrlQuery request, CancellationToken cancellationToken)
        {
            string url = request.url;
            var newsDTO = _mapper.Map<NewsDTO>(await _repositoryWrapper.NewsRepository.GetFirstOrDefaultAsync(
                predicate: sc => sc.URL == url,
                include: scl => scl
                    .Include(sc => sc.Image)));

            if (newsDTO is null)
            {
                return Result.Fail($"No news by entered Url - {url}");
            }

            if (newsDTO.Image is not null)
            {
                newsDTO.Image.Base64 = _blobService.FindFileInStorageAsBase64(newsDTO.Image.BlobName);
            }

            var news = (await _repositoryWrapper.NewsRepository.GetAllAsync()).ToList();
            var newsIndex = news.FindIndex(x => x.Id == newsDTO.Id);
            string prevNewsLink = null;
            string nextNewsLink = null;

            if(newsIndex != 0)
            {
                prevNewsLink = news[newsIndex - 1].URL;
            }

            if(newsIndex != news.Count - 1)
            {
                nextNewsLink = news[newsIndex + 1].URL;
            }

            var randomNewsTitleAndLink = new RandomNewsDTO();

            var arrCount = news.Count;
            if (arrCount > 3)
            {
                if (newsIndex + 1 == arrCount - 1 || newsIndex == arrCount - 1)
                {
                    var randomNews = await
                        _repositoryWrapper
                        .NewsRepository
                        .GetFirstOrDefaultAsync(
                            predicate: sc => sc.Id == news[newsIndex - 2].Id);
                    randomNewsTitleAndLink.RandomNewsUrl = randomNews.URL;
                    randomNewsTitleAndLink.Title = randomNews.Title;
                }
                else
                {
                    var randomNews = await
                        _repositoryWrapper
                        .NewsRepository
                        .GetFirstOrDefaultAsync(
                            predicate: sc => sc.Id == news[arrCount - 1].Id);
                    randomNewsTitleAndLink.RandomNewsUrl = randomNews.URL;
                    randomNewsTitleAndLink.Title = randomNews.Title;
                }
            }
            else
            {
                var randomNews = await
                        _repositoryWrapper
                        .NewsRepository
                        .GetFirstOrDefaultAsync(
                            predicate: sc => sc.Id == news[newsIndex].Id);
                randomNewsTitleAndLink.RandomNewsUrl = randomNews.URL;
                randomNewsTitleAndLink.Title = randomNews.Title;
            }

            var newsDTOWithUrls = new NewsDTOWithURLs();
            newsDTOWithUrls.RandomNews = randomNewsTitleAndLink;
            newsDTOWithUrls.News = newsDTO;
            newsDTOWithUrls.NextNewsUrl = nextNewsLink;
            newsDTOWithUrls.PrevNewsUrl = prevNewsLink;

            if (newsDTOWithUrls is null)
            {
                return Result.Fail($"No news by entered Url - {url}");
            }

            return Result.Ok(newsDTOWithUrls);
        }
    }
}