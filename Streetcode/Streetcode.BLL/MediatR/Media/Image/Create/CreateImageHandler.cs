using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.Interfaces.ImageComparator;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Media.Image.Create;

public class CreateImageHandler : IRequestHandler<CreateImageCommand, Result<ImageDTO>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IBlobService _blobService;
    private readonly ILoggerService _logger;
    private readonly IStringLocalizer<FailedToCreateSharedResource> _stringLocalizer;
    private readonly IStringLocalizer<CannotConvertNullSharedResource> _stringLocalizerCannot;
    private readonly IImageComparatorService _imageComparatorService;

    public CreateImageHandler(
        IBlobService blobService,
        IRepositoryWrapper repositoryWrapper,
        ILoggerService logger,
        IMapper mapper,
        IStringLocalizer<FailedToCreateSharedResource> stringLocalizer,
        IStringLocalizer<CannotConvertNullSharedResource> stringLocalizerCannot,
        IImageComparatorService imageComparatorService)
    {
        _blobService = blobService;
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _logger = logger;
        _stringLocalizer = stringLocalizer;
        _stringLocalizerCannot = stringLocalizerCannot;
        _imageComparatorService = imageComparatorService;
    }

    public async Task<Result<ImageDTO>> Handle(CreateImageCommand request, CancellationToken cancellationToken)
    {
        ulong imageHash = _imageComparatorService.SetImageHash(request.Image.BaseFormat);
        var existingImage = await TryFindExistingImage(imageHash);

        if (existingImage != null)
        {
            var existingImageDto = _mapper.Map<ImageDTO>(existingImage);

            existingImageDto.Base64 = _blobService.FindFileInStorageAsBase64(existingImageDto.BlobName);
            existingImageDto.ImageHash = imageHash;

            return Result.Ok(existingImageDto);
        }

        string hashBlobStorageName = _blobService.SaveFileInStorage(
            request.Image.BaseFormat,
            request.Image.Title,
            request.Image.Extension);

        var image = _mapper.Map<DAL.Entities.Media.Images.Image>(request.Image);

        image.BlobName = $"{hashBlobStorageName}.{request.Image.Extension}";
        image.ImageHash = imageHash;

        await _repositoryWrapper.ImageRepository.CreateAsync(image);
        var resultIsSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;

        var createdImage = _mapper.Map<ImageDTO>(image);

        createdImage.Base64 = _blobService.FindFileInStorageAsBase64(createdImage.BlobName);
        createdImage.ImageHash = imageHash;

        if(resultIsSuccess)
        {
            return Result.Ok(createdImage);
        }
        else
        {
            string errorMsg = _stringLocalizer["FailedToCreateAnImage"].Value;
            _logger.LogError(request, errorMsg);
            return Result.Fail(new Error(errorMsg));
        }
    }

    private async Task<DAL.Entities.Media.Images.Image?> TryFindExistingImage(ulong imageHash)
    {
        var existentImages = await _repositoryWrapper.ImageRepository.GetAllAsync();

        foreach (var image in existentImages)
        {
            if (imageHash == image.ImageHash)
            {
                return image;
            }
        }

        return null;
    }
}
