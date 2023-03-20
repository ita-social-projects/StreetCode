using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Media.Image.GetBaseFile;

public class GetBaseFileByNameHandler : IRequestHandler<GetBaseFileByNameQuery, Result<MemoryStream>>
{
    private readonly IBlobService _blobStorage;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public GetBaseFileByNameHandler(IBlobService blobService, IRepositoryWrapper repositoryWrapper)
    {
        _blobStorage = blobService;
        _repositoryWrapper = repositoryWrapper;
    }

    public async Task<Result<MemoryStream>> Handle(GetBaseFileByNameQuery request, CancellationToken cancellationToken)
    {
        var file = _blobStorage.FindFileInStorage(request.Name);

        return file;
    }
}
