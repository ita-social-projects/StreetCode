﻿using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Media;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Media.Audio.Create;

public class CreateAudioHandler : IRequestHandler<CreateAudioCommand, Result<AudioDTO>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IBlobService _blobService;

    public CreateAudioHandler(
        IBlobService blobService,
        IRepositoryWrapper repositoryWrapper,
        IMapper mapper)
    {
        _blobService = blobService;
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
    }

    public async Task<Result<AudioDTO>> Handle(CreateAudioCommand request, CancellationToken cancellationToken)
    {
        string hashBlobStorageName = _blobService.SaveFileInStorage(
            request.Audio.BaseFormat,
            request.Audio.Title,
            request.Audio.Extension);

        var audio = _mapper.Map<DAL.Entities.Media.Audio>(request.Audio);

        audio.BlobName = $"{hashBlobStorageName}.{request.Audio.Extension}";

        await _repositoryWrapper.AudioRepository.CreateAsync(audio);

        var resultIsSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;

        var createdAudio = _mapper.Map<AudioDTO>(audio);

        return resultIsSuccess ? Result.Ok(createdAudio) : Result.Fail(new Error("Failed to create an audio"));
    }
}
