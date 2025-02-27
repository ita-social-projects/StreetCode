using System.Linq.Expressions;
using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Media.Audio;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Services.EntityAccessManager;
using Streetcode.BLL.Services.EntityAccessManagerService;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Media.Audio.GetAll;

public class GetAllAudiosHandler : IRequestHandler<GetAllAudiosQuery, Result<IEnumerable<AudioDTO>>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IBlobService _blobService;
    private readonly ILoggerService _logger;
    private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;

    public GetAllAudiosHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, IBlobService blobService, ILoggerService logger, IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _blobService = blobService;
        _logger = logger;
        _stringLocalizerCannotFind = stringLocalizerCannotFind;
    }

    public async Task<Result<IEnumerable<AudioDTO>>> Handle(GetAllAudiosQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<DAL.Entities.Media.Audio, bool>>? basePredicate = null;

        var predicate = basePredicate.ExtendWithAccessPredicate(new StreetcodeAccessManager(), request.UserRole, a => a.Streetcode);

        var audios = await _repositoryWrapper.AudioRepository.GetAllAsync(predicate: predicate);

        if (audios is null)
        {
            string errorMsg = _stringLocalizerCannotFind["CannotFindAnyAudios"].Value;
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