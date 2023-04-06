﻿using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Media;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Media.Audio.GetById;

public class GetAudioByIdHandler : IRequestHandler<GetAudioByIdQuery, Result<AudioDTO>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IBlobService _blobService;

    public GetAudioByIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, IBlobService blobService)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _blobService = blobService;
    }

    public async Task<Result<AudioDTO>> Handle(GetAudioByIdQuery request, CancellationToken cancellationToken)
    {
        var audio = await _repositoryWrapper.AudioRepository.GetFirstOrDefaultAsync(f => f.Id == request.Id);

        if (audio is null)
        {
            return Result.Fail(new Error($"Cannot find an audio with corresponding id: {request.Id}"));
        }

        var audioDto = _mapper.Map<AudioDTO>(audio);

        audioDto.Base64 = _blobService.FindFileInStorageAsBase64(audioDto.BlobName);

        return Result.Ok(audioDto);
    }
}