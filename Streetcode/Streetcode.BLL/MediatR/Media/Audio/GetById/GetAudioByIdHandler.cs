using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Media;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Media.Audio.GetById;

public class GetAudioByIdHandler : IRequestHandler<GetAudioByIdQuery, Result<AudioDTO>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public GetAudioByIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
    }

    public async Task<Result<AudioDTO>> Handle(GetAudioByIdQuery request, CancellationToken cancellationToken)
    {
        var audio = await _repositoryWrapper.AudioRepository.GetFirstOrDefaultAsync(f => f.Id == request.Id);

        if (audio is null)
        {
            return Result.Fail(new Error($"Cannot find a Audio with corresponding Id: {request.Id}"));
        }

        var audioDto = _mapper.Map<AudioDTO>(audio);
        return Result.Ok(audioDto);
    }
}