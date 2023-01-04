using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Media;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Media.Audio.GetAll;

public class GetAllAudioHandler : IRequestHandler<GetAllAudeoQuery, Result<IEnumerable<AudioDTO>>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public GetAllAudioHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<AudioDTO>>> Handle(GetAllAudeoQuery request, CancellationToken cancellationToken)
    {
        var audio = await _repositoryWrapper.AudioRepository.GetAllAsync();

        var audioDtos = _mapper.Map<IEnumerable<AudioDTO>>(audio);
        return Result.Ok(audioDtos);
    }
}