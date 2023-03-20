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
        var audio = await _repositoryWrapper.AudioRepository.GetFirstOrDefaultAsync(f => f.Id == request.Name);

        if (audio is null)
        {
            return Result.Fail(new Error($"Cannot find an audio with corresponding id: {request.Name}"));
        }

        var file = _blobStorage.FindFileInStorage(audio.BlobStorageName);

        return file;
    }
}
