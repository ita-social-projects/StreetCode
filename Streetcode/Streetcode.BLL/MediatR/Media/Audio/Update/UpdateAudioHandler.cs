using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Media.Audio;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Media.Audio.Update;

public class UpdateAudioHandler : IRequestHandler<UpdateAudioCommand, Result<AudioDTO>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IBlobService _blobService;

    public UpdateAudioHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper, IBlobService blobService)
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
        _blobService = blobService;
    }

    public async Task<Result<AudioDTO>> Handle(UpdateAudioCommand request, CancellationToken cancellationToken)
    {
        var existingAudio = await _repositoryWrapper.AudioRepository
                .GetFirstOrDefaultAsync(a => a.Id == request.Audio.Id);

        if (existingAudio is null)
        {
            return Result.Fail(new Error($"Cannot find an audio with corresponding categoryId: {request.Audio.Id}"));
        }

        var updatedAudio = _mapper.Map<DAL.Entities.Media.Audio>(request.Audio);

        string newName = _blobService.UpdateFileInStorage(
            existingAudio.BlobName,
            request.Audio.BaseFormat,
            request.Audio.Title,
            request.Audio.Extension);

        updatedAudio.BlobName = $"{newName}.{request.Audio.Extension}";

        _repositoryWrapper.AudioRepository.Update(updatedAudio);

        var resultIsSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;

        var createdAudio = _mapper.Map<AudioDTO>(updatedAudio);

        return resultIsSuccess ? Result.Ok(createdAudio) : Result.Fail(new Error("Failed to update an audio"));
    }
}
