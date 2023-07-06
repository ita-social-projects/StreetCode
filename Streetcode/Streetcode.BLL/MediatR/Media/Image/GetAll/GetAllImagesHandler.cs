using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Media.Image.GetAll;

public class GetAllImagesHandler : IRequestHandler<GetAllImagesQuery, Result<IEnumerable<ImageDTO>>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IBlobService _blobService;
    private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;

    public GetAllImagesHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, IBlobService blobService, IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _blobService = blobService;
        _stringLocalizerCannotFind = stringLocalizerCannotFind;
    }

    public async Task<Result<IEnumerable<ImageDTO>>> Handle(GetAllImagesQuery request, CancellationToken cancellationToken)
    {
        var images = await _repositoryWrapper.ImageRepository.GetAllAsync();

        if (images is null)
        {
            return Result.Fail(new Error(_stringLocalizerCannotFind["CannotFindAnyImage"].Value));
        }

        var imageDtos = _mapper.Map<IEnumerable<ImageDTO>>(images);

        foreach (var image in imageDtos)
        {
            image.Base64 = _blobService.FindFileInStorageAsBase64(image.BlobName);
        }

        return Result.Ok(imageDtos);
    }
}