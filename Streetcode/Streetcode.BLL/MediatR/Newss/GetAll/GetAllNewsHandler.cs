using System.Linq.Expressions;
using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.DTO.News;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.Services.EntityAccessManager;
using Streetcode.DAL.Entities.News;
using Streetcode.DAL.Helpers;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Newss.GetAll
{
    public class GetAllNewsHandler : IRequestHandler<GetAllNewsQuery, Result<GetAllNewsResponseDTO>>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly IBlobService _blobService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetAllNewsHandler(
            IRepositoryWrapper repositoryWrapper,
            IMapper mapper,
            IBlobService blobService,
            IHttpContextAccessor httpContextAccessor)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _blobService = blobService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<GetAllNewsResponseDTO>> Handle(GetAllNewsQuery request, CancellationToken cancellationToken)
        {
            var searchTitle = request.title?.Trim().ToLower();
            Expression<Func<News, bool>>? basePredicate = null;
            var predicate = basePredicate.ExtendWithAccessPredicate(new NewsAccessManager(), request.UserRole);

            var allNews = await _repositoryWrapper
                .NewsRepository
                .GetAllAsync(
                    predicate: predicate,
                    include: newsCollection => newsCollection.Include(news => news.Image!));

            var filteredNews = string.IsNullOrWhiteSpace(searchTitle)
                ? allNews
                : allNews
                    .Where(news =>
                        !string.IsNullOrWhiteSpace(news.Title) &&
                        news.Title.ToLower().Contains(searchTitle))
                    .ToList();

            int page = request.page ?? 1;
            int pageSize = request.pageSize ?? 10;

            int totalItems = filteredNews.Count();

            var paginatedNews = filteredNews
                .OrderByDescending(news => news.CreationDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var newsDTOs = MapToNewsDTOs(paginatedNews);

            var getAllNewsResponseDTO = new GetAllNewsResponseDTO
            {
                TotalAmount = totalItems,
                News = newsDTOs
            };

            return Result.Ok(getAllNewsResponseDTO);
        }

        private IEnumerable<NewsDTO> MapToNewsDTOs(IEnumerable<News> entities)
        {
            var newsDTOs = _mapper.Map<IEnumerable<NewsDTO>>(entities);
            foreach (var dto in newsDTOs)
            {
                if (dto.Image is not null)
                {
                    dto.Image.Base64 = _blobService.FindFileInStorageAsBase64(dto.Image.BlobName);
                }
            }

            return newsDTOs;
        }

        private void AddPaginationHeader(ushort pageSize, ushort currentPage, ushort totalPages, ushort totalItems)
        {
            var metadata = new
            {
                CurrentPage = currentPage,
                TotalPages = totalPages,
                PageSize = pageSize,
                TotalItems = totalItems
            };

            _httpContextAccessor.HttpContext!.Response.Headers.Add(
                "X-Pagination",
                Newtonsoft.Json.JsonConvert.SerializeObject(metadata));
        }
    }
}