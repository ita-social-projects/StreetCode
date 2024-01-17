using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Media.Audio.Delete;

public class DeleteAudioHandler : IRequestHandler<DeleteAudioCommand, Result<Unit>>
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IBlobService _blobService;
    private readonly ILoggerService _logger;
    private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;
    private readonly IStringLocalizer<FailedToDeleteSharedResource> _stringLocalizerFailedToDelete;

    public DeleteAudioHandler(
        IRepositoryWrapper repositoryWrapper,
        IBlobService blobService,
        ILoggerService logger,
        IStringLocalizer<FailedToDeleteSharedResource> stringLocalizerFailedToDelete,
        IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind)
    {
        _repositoryWrapper = repositoryWrapper;
        _blobService = blobService;
        _logger = logger;
        _stringLocalizerFailedToDelete = stringLocalizerFailedToDelete;
        _stringLocalizerCannotFind = stringLocalizerCannotFind;
    }

    public async Task<Result<Unit>> Handle(DeleteAudioCommand request, CancellationToken cancellationToken)
    {
        var audio = await _repositoryWrapper.AudioRepository.GetFirstOrDefaultAsync(a => a.Id == request.Id);

        if (audio is null)
        {
            string errorMsg = _stringLocalizerCannotFind["CannotFindAnAudioWithCorrespondingCategoryId", request.Id].Value;
            _logger.LogError(request, errorMsg);
            return Result.Fail(new Error(errorMsg));
        }

        _repositoryWrapper.AudioRepository.Delete(audio);

        var resultIsSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;

        if (resultIsSuccess)
        {
            _blobService.DeleteFileInStorage(audio.BlobName);
        }

        if (resultIsSuccess)
        {
            _logger?.LogInformation($"DeleteAudioCommand handled successfully");
            return Result.Ok(Unit.Value);
        }
        else
        {
            string errorMsg = _stringLocalizerFailedToDelete["FailedToDeleteAudio"].Value;
            _logger.LogError(request, errorMsg);
            return Result.Fail(new Error(errorMsg));
        }
    }
}
