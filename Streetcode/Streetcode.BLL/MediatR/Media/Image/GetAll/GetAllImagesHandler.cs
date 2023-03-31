using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Media.Image.GetAll;

public class GetAllImagesHandler : IRequestHandler<GetAllImagesQuery, Result<IEnumerable<ImageDTO>>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IBlobService _blobService;

    public GetAllImagesHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, IBlobService blobService)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _blobService = blobService;
    }

    public async Task<Result<IEnumerable<ImageDTO>>> Handle(GetAllImagesQuery request, CancellationToken cancellationToken)
    {
        var images = await _repositoryWrapper.ImageRepository.GetAllAsync();

        if (images is null)
        {
            return Result.Fail(new Error($"Cannot find any image"));
        }

        var imageDtos = _mapper.Map<IEnumerable<ImageDTO>>(images);

        foreach (var image in imageDtos)
        {
            image.Base64 = _blobService.FindFileInStorageAsBase64(image.BlobName);
        }

        return Result.Ok(imageDtos);
    }
}