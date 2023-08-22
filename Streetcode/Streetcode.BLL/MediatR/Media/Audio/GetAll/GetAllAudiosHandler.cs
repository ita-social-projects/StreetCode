using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Media.Audio;
using Streetcode.BLL.DTO.Media;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Media.Audio.GetAll;

public class GetAllAudiosHandler : IRequestHandler<GetAllAudiosQuery, Result<IEnumerable<AudioDTO>>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IBlobService _blobService;
    private readonly ILoggerService _logger;

    public GetAllAudiosHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, IBlobService blobService, ILoggerService logger)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _blobService = blobService;
        _logger = logger;
    }

    public async Task<Result<IEnumerable<AudioDTO>>> Handle(GetAllAudiosQuery request, CancellationToken cancellationToken)
    {
        var audios = await _repositoryWrapper.AudioRepository.GetAllAsync();

        if (audios is null)
        {
            const string errorMsg = "Cannot find any audios";
            _logger.LogError(request, errorMsg);
            return Result.Fail(new Error(errorMsg));
        }

        var audioDtos = _mapper.Map<IEnumerable<AudioDTO>>(audios);
        foreach (var audio in audioDtos)
        {
            audio.Base64 = _blobService.FindFileInStorageAsBase64(audio.BlobName);
        }

        return Result.Ok(audioDtos);
    }
}