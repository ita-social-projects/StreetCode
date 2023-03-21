using FluentResults;
using MediatR;
using Streetcode.BLL.Interfaces.BlobStorage;

namespace Streetcode.BLL.MediatR.Media.Audio.GetBaseAudio;

public class GetBaseAudioHandler : IRequestHandler<GetBaseAudioQuery, Result<MemoryStream>>
{
    private readonly IBlobService _blobStorage;

    public GetBaseAudioHandler(IBlobService blobService)
    {
        _blobStorage = blobService;
    }

    public async Task<Result<MemoryStream>> Handle(GetBaseAudioQuery request, CancellationToken cancellationToken)
    {
        var audioFile = _blobStorage.FindFileInStorage(request.Name);

        return audioFile;
    }
}
