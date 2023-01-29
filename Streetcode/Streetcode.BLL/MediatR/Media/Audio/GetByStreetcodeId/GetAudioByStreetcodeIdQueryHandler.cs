using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Media;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Media.Audio.GetByStreetcodeId;

public class GetAudioByStreetcodeIdQueryHandler : IRequestHandler<GetAudioByStreetcodeIdQuery, Result<AudioDTO>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public GetAudioByStreetcodeIdQueryHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
    }

    public async Task<Result<AudioDTO>> Handle(GetAudioByStreetcodeIdQuery request, CancellationToken cancellationToken)
    {
        var audio = await _repositoryWrapper.AudioRepository
            .GetFirstOrDefaultAsync(audio => audio.StreetcodeId == request.streetcodeId);

        if (audio is null)
        {
            return Result.Fail(new Error($"Cannot find an audio with the corresponding streetcode id: {request.streetcodeId}"));
        }

        var audioDto = _mapper.Map<AudioDTO>(audio);
        return Result.Ok(audioDto);
    }
}