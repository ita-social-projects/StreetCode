using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Sources;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Sources.SourceLink.GetCategoryById;

public class GetCategoryByIdHandler : IRequestHandler<GetCategoryByIdQuery, Result<SourceLinkCategoryDto>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IBlobService _blobService;
    private readonly ILoggerService _logger;
    private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;

    public GetCategoryByIdHandler(
        IRepositoryWrapper repositoryWrapper,
        IMapper mapper,
        IBlobService blobService,
        ILoggerService logger,
        IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _blobService = blobService;
        _logger = logger;
        _stringLocalizerCannotFind = stringLocalizerCannotFind;
    }

    public async Task<Result<SourceLinkCategoryDto>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var srcCategories = await _repositoryWrapper
            .SourceCategoryRepository
            .GetFirstOrDefaultAsync(
                predicate: sc => sc.Id == request.Id,
                include: scl => scl
                    .Include(sc => sc.StreetcodeCategoryContents)
                    .Include(sc => sc.Image) !);

        if (srcCategories is null)
        {
            string errorMsg = _stringLocalizerCannotFind["CannotFindAnySrcCategoryByTheCorrespondingId", request.Id].Value;
            _logger.LogError(request, errorMsg);
            return Result.Fail(new Error(errorMsg));
        }

        var mappedSrcCategories = _mapper.Map<SourceLinkCategoryDto>(srcCategories);

        mappedSrcCategories.Image.Base64 = _blobService.FindFileInStorageAsBase64(mappedSrcCategories.Image.BlobName);

        return Result.Ok(mappedSrcCategories);
    }
}