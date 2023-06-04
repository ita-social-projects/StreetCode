using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Media.Audio;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Media.Audio.GetByStreetcodeId;

public class GetAudioByStreetcodeIdQueryHandler : IRequestHandler<GetAudioByStreetcodeIdQuery, Result<AudioDTO>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IBlobService _blobService;

    public GetAudioByStreetcodeIdQueryHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, IBlobService blobService)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _blobService = blobService;
    }

    public async Task<Result<AudioDTO>> Handle(GetAudioByStreetcodeIdQuery request, CancellationToken cancellationToken)
    {
        var audio = await _repositoryWrapper.AudioRepository
            .GetFirstOrDefaultAsync(audio => audio.Streetcode.Id == request.StreetcodeId);

        if (audio is null)
        {
            return Result.Fail(new Error($"Cannot find an audio with the corresponding streetcode id: {request.StreetcodeId}"));
        }

        var audioDto = _mapper.Map<AudioDTO>(audio);

        audioDto.Base64 = _blobService.FindFileInStorageAsBase64(audioDto.BlobName);

        return Result.Ok(audioDto);
    }
}