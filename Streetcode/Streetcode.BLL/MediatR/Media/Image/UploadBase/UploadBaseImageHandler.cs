using FluentResults;
using MediatR;
using Streetcode.BLL.Interfaces.BlobStorage;

namespace Streetcode.BLL.MediatR.Media.Image.UploadBase;

public class UploadBaseImageHandler : IRequestHandler<UploadBaseImageCommand, Result<Unit>>
{
    private readonly IBlobService _blobService;
    public UploadBaseImageHandler(IBlobService blobService)
    {
        _blobService = blobService;
    }

    public async Task<Result<Unit>> Handle(UploadBaseImageCommand request, CancellationToken cancellationToken)
    {
        _blobService.SaveFileInStorage(
            request.Image.BaseFormat,
            request.Image.Title,
            request.Image.MimeType);

        return Result.Ok(Unit.Value);
    }
}
