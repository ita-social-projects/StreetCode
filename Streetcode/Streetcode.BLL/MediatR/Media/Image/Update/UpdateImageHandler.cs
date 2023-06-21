using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Media.Image.Update;

public class UpdateImageHandler : IRequestHandler<UpdateImageCommand, Result<ImageDTO>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IBlobService _blobService;
    private readonly ILoggerService? _logger;

    public UpdateImageHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper, IBlobService blobService, ILoggerService? logger)
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
        _blobService = blobService;
        _logger = logger;
    }

    public async Task<Result<ImageDTO>> Handle(UpdateImageCommand request, CancellationToken cancellationToken)
    {
        var existingImage = await _repositoryWrapper.ImageRepository
                .GetFirstOrDefaultAsync(a => a.Id == request.Image.Id);

        if (existingImage is null)
        {
            string errorMsg = $"Cannot find an image with corresponding categoryId: {request.Image.Id}";
            _logger?.LogError("UpdateImageCommand handled with an error");
            _logger?.LogError(errorMsg);
            return Result.Fail(new Error(errorMsg));
        }

        var updatedImage = _mapper.Map<DAL.Entities.Media.Images.Image>(request.Image);

        string newName = _blobService.UpdateFileInStorage(
            existingImage.BlobName,
            request.Image.BaseFormat,
            request.Image.Title,
            request.Image.Extension);

        updatedImage.BlobName = $"{newName}.{request.Image.Extension}";

        _repositoryWrapper.ImageRepository.Update(updatedImage);

        var resultIsSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;

        var returnedImaged = _mapper.Map<ImageDTO>(updatedImage);

        returnedImaged.Base64 = _blobService.FindFileInStorageAsBase64(returnedImaged.BlobName);

        if(resultIsSuccess)
        {
            _logger?.LogInformation($"UpdateImageCommand handled successfully");
            return Result.Ok(returnedImaged);
        }
        else
        {
            const string errorMsg = $"Failed to update an image";
            _logger?.LogError("UpdateImageCommand handled with an error");
            _logger?.LogError(errorMsg);
            return Result.Fail(new Error(errorMsg));
        }
    }
}
