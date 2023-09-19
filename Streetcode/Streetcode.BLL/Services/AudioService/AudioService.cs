using Repositories.Interfaces;
using Streetcode.BLL.Interfaces.Audio;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.DAL.Repositories.Interfaces.Streetcode;

namespace Streetcode.BLL.Services.AudioService;

public class AudioService : IAudioService
{
    private readonly ILoggerService _loggerService;
    private readonly IAudioRepository _audioRepository;
    private readonly IStreetcodeRepository _streetcodeRepository;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IBlobService _blobService;

    public AudioService(ILoggerService loggerService, IRepositoryWrapper repositoryWrapper, IBlobService blobService)
    {
        _loggerService = loggerService;
        _repositoryWrapper = repositoryWrapper;
        _audioRepository = repositoryWrapper.AudioRepository;
        _streetcodeRepository = repositoryWrapper.StreetcodeRepository;
        _blobService = blobService;
    }

    public async Task CleanUnusedAudios()
    {
        var referencedAudioIds = _streetcodeRepository
            .FindAll()
            .Where(streetcode => streetcode.AudioId.HasValue)
            .Select(streetcode => streetcode.AudioId.Value)
            .ToList();

        var unreferencedAudios = _audioRepository
            .FindAll(audio => !referencedAudioIds.Contains(audio.Id))
            .ToList();

        _loggerService.LogInformation("Starting deleting this audios:");

        foreach (var audio in unreferencedAudios)
        {
            _loggerService.LogInformation(audio.BlobName);
        }

        if (unreferencedAudios.Any())
        {
            foreach (var audio in unreferencedAudios)
            {
                _blobService.DeleteFileInStorage(audio.BlobName);
            }

            _audioRepository.DeleteRange(unreferencedAudios);
            await _repositoryWrapper.SaveChangesAsync();
        }

        _loggerService.LogInformation("Deleting completed:");
    }
}