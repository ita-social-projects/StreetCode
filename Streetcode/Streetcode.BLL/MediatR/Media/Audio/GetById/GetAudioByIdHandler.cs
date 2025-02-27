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

namespace Streetcode.BLL.MediatR.Media.Audio.GetById;

public class GetAudioByIdHandler : IRequestHandler<GetAudioByIdQuery, Result<AudioDTO>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IBlobService _blobService;
    private readonly ILoggerService _logger;
    private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;

    public GetAudioByIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, IBlobService blobService, ILoggerService logger, IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _blobService = blobService;
        _logger = logger;
        _stringLocalizerCannotFind = stringLocalizerCannotFind;
    }

    public async Task<Result<AudioDTO>> Handle(GetAudioByIdQuery request, CancellationToken cancellationToken)
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

        var audioDto = _mapper.Map<AudioDTO>(audio);

        audioDto.Base64 = _blobService.FindFileInStorageAsBase64(audioDto.BlobName);

        return Result.Ok(audioDto);
    }
}