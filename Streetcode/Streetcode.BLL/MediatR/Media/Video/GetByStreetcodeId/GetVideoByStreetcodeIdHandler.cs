using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Media.Video;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Media.Video.GetByStreetcodeId;

public class GetVideoByStreetcodeIdHandler : IRequestHandler<GetVideoByStreetcodeIdQuery, Result<VideoDTO>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public GetVideoByStreetcodeIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
    }

    public async Task<Result<VideoDTO>> Handle(GetVideoByStreetcodeIdQuery request, CancellationToken cancellationToken)
    {
        StreetcodeContent? streetcode = await _repositoryWrapper.StreetcodeRepository.GetFirstOrDefaultAsync(x => x.Id == request.StreetcodeId);
        if(streetcode is null)
        {
            return Result.Fail(new Error($"Streetcode with id: {request.StreetcodeId} doesn`t exist"));
        }

        var video = await _repositoryWrapper.VideoRepository
            .GetFirstOrDefaultAsync(video => video.StreetcodeId == request.StreetcodeId);

        if (video is null)
        {
            return Result.Fail(new Error($"Cannot find any video by the streetcode id: {request.StreetcodeId}"));
        }

        var videoDto = _mapper.Map<VideoDTO>(video);
        return Result.Ok(videoDto);
    }
}