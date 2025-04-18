using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Sources;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.Interfaces.Logging;
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

        public GetAllCategoriesHandler(
            IRepositoryWrapper repositoryWrapper,
            IMapper mapper,
            IBlobService blobService,
            ILoggerService logger,
            IStringLocalizer<NoSharedResource> stringLocalizerNo)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _blobService = blobService;
            _logger = logger;
            _stringLocalizerNo = stringLocalizerNo;
        }

        public async Task<Result<GetAllCategoriesResponseDTO>> Handle(
            GetAllCategoriesQuery request,
            CancellationToken cancellationToken)
        {
            var allCategories = await _repositoryWrapper.SourceCategoryRepository
                .FindAll(
                    include: cat => cat.Include(img => img.Image)!,
                    predicate: cat => string.IsNullOrWhiteSpace(request.title) ||
                                      cat.Title.ToLower().Contains(request.title.ToLower()))
                .ToListAsync(cancellationToken);

            var page = request.page ?? 1;
            var pageSize = request.pageSize ?? 10;

            var paginatedCategories = allCategories
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var dtos = _mapper.Map<IEnumerable<SourceLinkCategoryDTO>>(paginatedCategories);

            foreach (var dto in dtos)
            {
                if (dto.Image is not null)
                {
                    dto.Image.Base64 = _blobService.FindFileInStorageAsBase64(dto.Image.BlobName);
                }
            }

            var getAllCategoriesResponseDTO = new GetAllCategoriesResponseDTO
            {
                TotalAmount = allCategories.Count(),
                Categories = dtos
            };

            return Result.Ok(getAllCategoriesResponseDTO);
        }
    }
}
