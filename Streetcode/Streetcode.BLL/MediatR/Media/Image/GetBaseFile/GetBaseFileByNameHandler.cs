using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.Interfaces.BlobStorage;

namespace Streetcode.BLL.MediatR.Media.Image.GetBaseFile;

public class GetBaseFileByNameHandler : IRequestHandler<GetBaseFileByNameQuery, Result<ImageBaseDTO>>
{
    private readonly IBlobService _blobStorage;

    public GetBaseFileByNameHandler(IBlobService blobService)
    {
        _blobStorage = blobService;
    }

    public async Task<Result<ImageBaseDTO>> Handle(GetBaseFileByNameQuery request, CancellationToken cancellationToken)
    {
        string encodedBase;
        string mimeType;

        (encodedBase, mimeType) = _blobStorage.FindFileInStorage(request.Name);

        return Result.Ok(new ImageBaseDTO()
        {
                BaseFormat = encodedBase,
                MimeType = mimeType,
                Name = request.Name
        });
    }
}
