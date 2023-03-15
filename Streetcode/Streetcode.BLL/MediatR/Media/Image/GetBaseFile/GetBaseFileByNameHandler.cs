using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.Interfaces.BlobStorage;

namespace Streetcode.BLL.MediatR.Media.Image.GetBaseFile;

public class GetBaseFileByNameHandler : IRequestHandler<GetBaseFileByNameQuery, Result<MemoryStream>>
{
    private readonly IBlobService _blobStorage;

    public GetBaseFileByNameHandler(IBlobService blobService)
    {
        _blobStorage = blobService;
    }

    public async Task<Result<MemoryStream>> Handle(GetBaseFileByNameQuery request, CancellationToken cancellationToken)
    {
        var file = _blobStorage.FindFileInStorage(request.Name);

        return file;
    }
}
