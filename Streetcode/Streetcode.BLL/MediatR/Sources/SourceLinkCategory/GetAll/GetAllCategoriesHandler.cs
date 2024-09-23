using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.DTO.Sources;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Services.BlobStorageService;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.News;
using Streetcode.DAL.Helpers;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Sources.SourceLinkCategory.GetAll
{
    public class GetAllCategoriesHandler : IRequestHandler<GetAllCategoriesQuery, Result<GetAllCategoriesResponseDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IBlobService _blobService;
        private readonly IStringLocalizer<NoSharedResource> _stringLocalizerNo;
        private readonly ILoggerService _logger;
        public GetAllCategoriesHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, IBlobService blobService, ILoggerService logger, IStringLocalizer<NoSharedResource> stringLocalizerNo)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _blobService = blobService;
            _logger = logger;
            _stringLocalizerNo = stringLocalizerNo;
        }

        public Task<Result<GetAllCategoriesResponseDTO>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationtoken)
        {
            PaginationResponse<DAL.Entities.Sources.SourceLinkCategory> paginationResponse = _repositoryWrapper.SourceCategoryRepository
                .GetAllPaginated(
                    request.page,
                    request.pageSize,
                    include: cat => cat.Include(img => img.Image) !,
                    descendingSortKeySelector: cat => cat.Title);

            if (paginationResponse == null)
            {
                string errorMsg = _stringLocalizerNo["NoCategories"].Value;
                _logger.LogError(request, errorMsg);
                return Task.FromResult(Result.Fail<GetAllCategoriesResponseDTO>(new Error(errorMsg)));
            }

            var dtos = _mapper.Map<IEnumerable<SourceLinkCategoryDTO>>(paginationResponse.Entities);

            foreach (var dto in dtos)
            {
                dto.Image.Base64 = _blobService.FindFileInStorageAsBase64(dto.Image.BlobName);
            }

            GetAllCategoriesResponseDTO getAllCategoriesResponseDTO = new GetAllCategoriesResponseDTO
            {
                TotalAmount = paginationResponse.TotalItems,
                Categories = dtos,
            };

            return Task.FromResult(Result.Ok(getAllCategoriesResponseDTO));
        }
    }
}
