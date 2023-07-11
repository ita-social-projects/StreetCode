using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Media.Image.Create;

public class CreateImageHandler : IRequestHandler<CreateImageCommand, Result<ImageDTO>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IBlobService _blobService;
    private readonly ILoggerService _logger;

    public CreateImageHandler(
        IBlobService blobService,
        IRepositoryWrapper repositoryWrapper,
        IMapper mapper,
        ILoggerService logger)
    {
        _blobService = blobService;
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<ImageDTO>> Handle(CreateImageCommand request, CancellationToken cancellationToken)
    {
        string hashBlobStorageName = _blobService.SaveFileInStorage(
            request.Image.BaseFormat,
            request.Image.Title,
            request.Image.Extension);

        var image = _mapper.Map<DAL.Entities.Media.Images.Image>(request.Image);

        image.BlobName = $"{hashBlobStorageName}.{request.Image.Extension}";

        await _repositoryWrapper.ImageRepository.CreateAsync(image);
        var resultIsSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;

        var createdImage = _mapper.Map<ImageDTO>(image);

        createdImage.Base64 = _blobService.FindFileInStorageAsBase64(createdImage.BlobName);

        if(resultIsSuccess)
        {
            return Result.Ok(createdImage);
        }
        else
        {
            const string errorMsg = "Failed to create an image";
            _logger.LogError(request, errorMsg);
            return Result.Fail(new Error(errorMsg));
        }
    }
}
