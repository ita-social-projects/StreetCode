using Streetcode.BLL.Interfaces.Audio;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.Services.AudioService;

public class AudioService : IAudioService
{
    private readonly ILoggerService _loggerService;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IBlobService _blobService;

    public AudioService(ILoggerService loggerService, IRepositoryWrapper repositoryWrapper, IBlobService blobService)
    {
        _loggerService = loggerService;
        _repositoryWrapper = repositoryWrapper;
        _blobService = blobService;
    }

    public async Task CleanUnusedAudiosAsync()
    {
        try
        {
            var referencedAudioIds = _repositoryWrapper.StreetcodeRepository
                .FindAll()
                .Where(streetcode => streetcode.AudioId.HasValue)
                .Select(streetcode => streetcode.AudioId.Value);

            var unreferencedAudios = _repositoryWrapper.AudioRepository
                .FindAll(audio => !referencedAudioIds.Contains(audio.Id))
                .ToList();

            _loggerService.LogInformation("Starting deleting this audios:");

            if (unreferencedAudios.Any())
            {
                foreach (var audio in unreferencedAudios)
                {
                    _loggerService.LogInformation(audio.BlobName!);
                    _blobService.DeleteFileInStorage(audio.BlobName!);
                }

                _repositoryWrapper.AudioRepository.DeleteRange(unreferencedAudios);
                await _repositoryWrapper.SaveChangesAsync();
            }

            _loggerService.LogInformation("Deleting completed.");
        }
        catch (Exception e)
        {
            _loggerService.LogError(e, e.Message);
            throw;
        }
    }
}