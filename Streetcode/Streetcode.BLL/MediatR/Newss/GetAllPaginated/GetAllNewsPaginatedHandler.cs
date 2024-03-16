using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.News;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.Interfaces.Logging;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.News;
using Streetcode.BLL.Util;

namespace Streetcode.BLL.MediatR.Newss.GetAllPaginated
{
    public class GetAllNewsPaginatedHandler : IRequestHandler<GetAllNewsPaginatedQuery, Result<IEnumerable<NewsDTO>>>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly IBlobService _blobService;
        private readonly ILoggerService _logger;
        private readonly IStringLocalizer<NoSharedResource> _stringLocalizerNo;

        public GetAllNewsPaginatedHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, IBlobService blobService, ILoggerService logger, IStringLocalizer<NoSharedResource> stringLocalizerNo)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _blobService = blobService;
            _logger = logger;
            _stringLocalizerNo = stringLocalizerNo;
        }

        public async Task<Result<IEnumerable<NewsDTO>>> Handle(GetAllNewsPaginatedQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<News>? news = await _repositoryWrapper.NewsRepository
                .GetAllAsync(include: cat => cat.Include(img => img.Image));
            IEnumerable<News>? paginatedNews = news?.Paginate(request.page, request.pageSize);

            if (paginatedNews is null || !paginatedNews.Any())
            {
                string errorMsg = _stringLocalizerNo["New's weren't found"].Value;
                _logger.LogError(request, errorMsg);
                return Result.Fail(errorMsg);
            }

            var newsDTOs = _mapper.Map<IEnumerable<NewsDTO>>(paginatedNews);

            foreach (var dto in newsDTOs)
            {
                if(dto.Image is not null)
                {
                    dto.Image.Base64 = _blobService.FindFileInStorageAsBase64(dto.Image.BlobName);
                }
            }

            return Result.Ok(newsDTOs);
        }
    }
}
