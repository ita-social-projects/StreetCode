using System.Linq.Expressions;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Services.EntityAccessManager;
using Streetcode.BLL.Services.EntityAccessManagerService;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Media.Audio.GetBaseAudio;

public class GetBaseAudioHandler : IRequestHandler<GetBaseAudioQuery, Result<MemoryStream>>
{
    private readonly IBlobService _blobStorage;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService _logger;
    private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;

    public GetBaseAudioHandler(IBlobService blobService, IRepositoryWrapper repositoryWrapper, ILoggerService logger, IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind)
    {
        _blobStorage = blobService;
        _repositoryWrapper = repositoryWrapper;
        _logger = logger;
        _stringLocalizerCannotFind = stringLocalizerCannotFind;
    }

    public async Task<Result<MemoryStream>> Handle(GetBaseAudioQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<DAL.Entities.Media.Audio, bool>>? basePredicate = a => a.Id == request.Id;

        var predicate = basePredicate.ExtendWithAccessPredicate(new StreetcodeAccessManager(), request.UserRole, a => a.Streetcode);

        var audio = await _repositoryWrapper.AudioRepository.GetFirstOrDefaultAsync(predicate: predicate);

        if (audio is null)
        {
            string errorMsg = _stringLocalizerCannotFind["CannotFindAnAudioWithCorrespondingId", request.Id].Value;
            _logger.LogError(request, errorMsg);
            return Result.Fail(new Error(errorMsg));
        }

        return _blobStorage.FindFileInStorageAsMemoryStream(audio.BlobName);
    }
}
