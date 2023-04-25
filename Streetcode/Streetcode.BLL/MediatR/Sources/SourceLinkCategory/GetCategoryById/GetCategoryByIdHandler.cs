using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.DTO.Sources;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.DAL.Entities.AdditionalContent.Coordinates;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Sources.SourceLink.GetCategoryById;

public class GetCategoryByIdHandler : IRequestHandler<GetCategoryByIdQuery, Result<SourceLinkCategoryDTO>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IBlobService _blobService;

    public GetCategoryByIdHandler(
        IRepositoryWrapper repositoryWrapper,
        IMapper mapper,
        IBlobService blobService)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _blobService = blobService;
    }

    public async Task<Result<SourceLinkCategoryDTO>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
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
            return Result.Fail(new Error($"Cannot find any srcCategory by the corresponding id: {request.Id}"));
        }

        var mappedSrcCategories = _mapper.Map<SourceLinkCategoryDTO>(srcCategories);

        mappedSrcCategories.Image.Base64 = _blobService.FindFileInStorageAsBase64(mappedSrcCategories.Image.BlobName);

        return Result.Ok(mappedSrcCategories);
    }
}