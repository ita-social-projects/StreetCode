using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Media.Image.Update;

public class UpdateImageHandler : IRequestHandler<UpdateImageCommand, Result<ImageDTO>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IBlobService _blobService;

    public UpdateImageHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper, IBlobService blobService)
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
        _blobService = blobService;
    }

    public async Task<Result<ImageDTO>> Handle(UpdateImageCommand request, CancellationToken cancellationToken)
    {
        var existingImage = await _repositoryWrapper.ImageRepository
                .GetFirstOrDefaultAsync(a => a.Id == request.Image.Id);

        if (existingImage is null)
        {
            return Result.Fail(new Error($"Cannot find an image with corresponding categoryId: {request.Image.Id}"));
        }

        var updatedImage = _mapper.Map<DAL.Entities.Media.Images.Image>(request.Image);

        UpdateFileInStorage(request, existingImage.BlobName, ref updatedImage);

        _repositoryWrapper.ImageRepository.Update(updatedImage);

        var resultIsSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;

        var returnedImaged = _mapper.Map<ImageDTO>(updatedImage);

        returnedImaged.Base64 = _blobService.FindFileInStorageAsBase64(returnedImaged.BlobName);

        return resultIsSuccess ? Result.Ok(returnedImaged) : Result.Fail(new Error("Failed to update an image"));
    }

    private void UpdateFileInStorage(
        UpdateImageCommand updatedAudioRequest,
        string previousBlobName,
        ref DAL.Entities.Media.Images.Image mappedAudio)
    {
        _blobService.DeleteFileInStorage(previousBlobName);

        string hashBlobStorageName = _blobService.SaveFileInStorage(
        updatedAudioRequest.Image.BaseFormat,
        updatedAudioRequest.Image.Title,
        updatedAudioRequest.Image.Extension);

        mappedAudio.BlobName = $"{hashBlobStorageName}.{updatedAudioRequest.Image.Extension}";
    }
}
