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

        return resultIsSuccess ? Result.Ok(returnedImaged) : Result.Fail(new Error("Failed to update an image"));
    }
}
