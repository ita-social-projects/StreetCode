using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Sources;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Sources.SourceLink.GetCategoriesByStreetcodeId;

public class GetCategoriesByStreetcodeIdHandler : IRequestHandler<GetCategoriesByStreetcodeIdQuery, Result<IEnumerable<SourceLinkCategoryDTO>>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IBlobService _blobService;
    private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;

    public GetCategoriesByStreetcodeIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, IBlobService blobService, IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _blobService = blobService;
        _stringLocalizerCannotFind = stringLocalizerCannotFind;
    }

    public async Task<Result<IEnumerable<SourceLinkCategoryDTO>>> Handle(GetCategoriesByStreetcodeIdQuery request, CancellationToken cancellationToken)
    {
        var srcCategories = await _repositoryWrapper
            .SourceCategoryRepository
            .GetAllAsync(
                predicate: sc => sc.Streetcodes.Any(s => s.Id == request.StreetcodeId),
                include: scl => scl.Include(sc => sc.Image) !);

        if (srcCategories is null)
        {
            return Result.Fail(new Error(_stringLocalizerCannotFind["CannotFindAnySourceCategoryWithTheStreetcodeId", request.StreetcodeId].Value));
        }

        var mappedSrcCategories = _mapper.Map<IEnumerable<SourceLinkCategoryDTO>>(srcCategories);

        foreach (var srcCategory in mappedSrcCategories)
        {
            srcCategory.Image.Base64 = _blobService.FindFileInStorageAsBase64(srcCategory.Image.BlobName);
        }

        return Result.Ok(mappedSrcCategories);
    }
}