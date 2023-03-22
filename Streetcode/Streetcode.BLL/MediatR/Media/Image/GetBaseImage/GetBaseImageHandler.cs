using FluentResults;
using MediatR;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Media.Image.GetBaseImage;

public class GetBaseImageHandler : IRequestHandler<GetBaseImageQuery, Result<MemoryStream>>
{
    private readonly IBlobService _blobStorage;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public GetBaseImageHandler(IBlobService blobService, IRepositoryWrapper repositoryWrapper)
    {
        _blobStorage = blobService;
        _repositoryWrapper = repositoryWrapper;
    }

    public async Task<Result<MemoryStream>> Handle(GetBaseImageQuery request, CancellationToken cancellationToken)
    {
        var image = await _repositoryWrapper.ImageRepository.GetFirstOrDefaultAsync(a => a.Id == request.Id);

        if (image is null)
        {
            return Result.Fail(new Error($"Cannot find an image with corresponding id: {request.Id}"));
        }

        var imageFile = _blobStorage.FindFileInStorage(image.BlobName);

        return imageFile;
    }
}
