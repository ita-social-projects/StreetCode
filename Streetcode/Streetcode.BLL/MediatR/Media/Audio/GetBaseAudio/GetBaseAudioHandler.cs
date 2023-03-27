using FluentResults;
using MediatR;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Media.Audio.GetBaseAudio;

public class GetBaseAudioHandler : IRequestHandler<GetBaseAudioQuery, Result<MemoryStream>>
{
    private readonly IBlobService _blobStorage;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public GetBaseAudioHandler(IBlobService blobService, IRepositoryWrapper repositoryWrapper)
    {
        _blobStorage = blobService;
        _repositoryWrapper = repositoryWrapper;
    }

    public async Task<Result<MemoryStream>> Handle(GetBaseAudioQuery request, CancellationToken cancellationToken)
    {
        var audio = await _repositoryWrapper.AudioRepository.GetFirstOrDefaultAsync(a => a.Id == request.Id);

        if (audio is null)
        {
            return Result.Fail(new Error($"Cannot find an audio with corresponding id: {request.Id}"));
        }

        var audioFile = _blobStorage.FindFileInStorageAsMemoryStream(audio.BlobName);

        return audioFile;
    }
}
