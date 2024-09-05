using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using Streetcode.BLL.DTO.Media.Audio;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Media.Audio.Create;

public class CreateAudioHandler : IRequestHandler<CreateAudioCommand, Result<AudioDTO>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IBlobService _blobService;
    private readonly ILoggerService _logger;
    private readonly IStringLocalizer<FailedToCreateSharedResource> _stringLocalizer;

    public CreateAudioHandler(
        IBlobService blobService,
        IRepositoryWrapper repositoryWrapper,
        ILoggerService logger,
        IMapper mapper,
        IStringLocalizer<FailedToCreateSharedResource> stringLocalizer)
    {
        _blobService = blobService;
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _logger = logger;
        _stringLocalizer = stringLocalizer;
    }

    public async Task<Result<AudioDTO>> Handle(CreateAudioCommand request, CancellationToken cancellationToken)
    {
        if (request.Audio.Extension.IsNullOrEmpty())
        {
            string? errorMsg = _stringLocalizer?["ExtensionIsRequired"].Value;
            _logger.LogError(request, errorMsg);
            return Result.Fail(new Error(errorMsg));
        }

        if (request.Audio.Title.IsNullOrEmpty())
        {
            string? errorMsg = _stringLocalizer?["TitleIsRequired"].Value;
            _logger.LogError(request, errorMsg);
            return Result.Fail(new Error(errorMsg));
        }

        string hashBlobStorageName = _blobService.SaveFileInStorage(
            request.Audio.BaseFormat,
            request.Audio.Title,
            request.Audio.Extension);

        var audio = _mapper.Map<DAL.Entities.Media.Audio>(request.Audio);

        audio.BlobName = $"{hashBlobStorageName}.{request.Audio.Extension}";

        await _repositoryWrapper.AudioRepository.CreateAsync(audio);

        var resultIsSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;

        var createdAudio = _mapper.Map<AudioDTO>(audio);

        createdAudio.Base64 = _blobService.FindFileInStorageAsBase64(createdAudio.BlobName);

        if (resultIsSuccess)
        {
            return Result.Ok(createdAudio);
        }
        else
        {
            string? errorMsg = _stringLocalizer?["FailedToCreateAudio"].Value;
            _logger.LogError(request, errorMsg);
            return Result.Fail(new Error(errorMsg));
        }
    }
}
