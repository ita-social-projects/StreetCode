using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.DTO.News;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.DAL.Entities.News;
using Streetcode.DAL.Helpers;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Newss.GetAll
{
    public class GetAllNewsHandler : IRequestHandler<GetAllNewsQuery, Result<GetAllNewsResponseDto>>
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

        public Task<Result<GetAllNewsResponseDto>> Handle(GetAllNewsQuery request, CancellationToken cancellationToken)
        {
            PaginationResponse<News> paginationResponse = _repositoryWrapper
                .NewsRepository
                .GetAllPaginated(
                    request.page,
                    request.pageSize,
                    include: newsCollection => newsCollection.Include(news => news.Image!),
                    descendingSortKeySelector: news => news.CreationDate);

            var newsDTOs = MapToNewsDTOs(paginationResponse.Entities);

            GetAllNewsResponseDto getAllNewsResponseDTO = new GetAllNewsResponseDto()
            {
                TotalAmount = paginationResponse.TotalItems,
                News = newsDTOs,
            };

            return Task.FromResult(Result.Ok(getAllNewsResponseDTO));
        }

        private IEnumerable<NewsDto> MapToNewsDTOs(IEnumerable<News> entities)
        {
            var newsDTOs = _mapper.Map<IEnumerable<NewsDto>>(entities);
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
