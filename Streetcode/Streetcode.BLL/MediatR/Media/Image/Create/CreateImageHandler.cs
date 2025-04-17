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
    private readonly IImageHashGeneratorService _imageHashGeneratorService;

    public CreateImageHandler(
        IBlobService blobService,
        IRepositoryWrapper repositoryWrapper,
        ILoggerService logger,
        IMapper mapper,
        IStringLocalizer<FailedToCreateSharedResource> stringLocalizer,
        IStringLocalizer<CannotConvertNullSharedResource> stringLocalizerCannot,
        IImageHashGeneratorService imageHashGeneratorService)
    {
        _blobService = blobService;
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _logger = logger;
        _stringLocalizer = stringLocalizer;
        _stringLocalizerCannot = stringLocalizerCannot;
        _imageHashGeneratorService = imageHashGeneratorService;
    }

    public async Task<Result<ImageDTO>> Handle(CreateImageCommand request, CancellationToken cancellationToken)
    {
        ulong imageHash = _imageHashGeneratorService.GenerateImageHash(request.Image.BaseFormat);

        var existingImage = await TryFindExistingImage(imageHash);

        if (existingImage != null)
        {
            var existingImageDto = MakeImageDto(existingImage);
            return Result.Ok(existingImageDto);
        }

        var newImage = MakeNewImage(request.Image, imageHash);

        await _repositoryWrapper.ImageRepository.CreateAsync(newImage);
        var resultIsSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;

        if(resultIsSuccess)
        {
            var createdImage = MakeImageDto(newImage);
            return Result.Ok(createdImage);
        }

        string errorMsg = _stringLocalizer["FailedToCreateAnImage"].Value;
        _logger.LogError(request, errorMsg);
        return Result.Fail(new Error(errorMsg));
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

    private ImageDTO MakeImageDto(DAL.Entities.Media.Images.Image image)
    {
        var imageDto = _mapper.Map<ImageDTO>(image);

        imageDto.Base64 = _blobService.FindFileInStorageAsBase64(imageDto.BlobName);
        imageDto.ImageHash = image.ImageHash;

        return imageDto;
    }

    private DAL.Entities.Media.Images.Image MakeNewImage(ImageFileBaseCreateDTO imageCreateDto, ulong imageHash)
    {
        string hashBlobStorageName = _blobService.SaveFileInStorage(
            imageCreateDto.BaseFormat,
            imageCreateDto.Title,
            imageCreateDto.Extension);

        var image = _mapper.Map<DAL.Entities.Media.Images.Image>(imageCreateDto);

        image.BlobName = $"{hashBlobStorageName}.{imageCreateDto.Extension}";
        image.ImageHash = imageHash;

        return image;
    }
}
