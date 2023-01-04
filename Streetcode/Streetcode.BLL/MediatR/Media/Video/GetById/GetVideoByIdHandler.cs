using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Media;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Media.Video.GetById;

public class GetVideoByIdHandler : IRequestHandler<GetVideoByIdQuery, Result<VideoDTO>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public GetVideoByIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
    }

    public async Task<Result<VideoDTO>> Handle(GetVideoByIdQuery request, CancellationToken cancellationToken)
    {
        var video = await _repositoryWrapper.VideoRepository.GetFirstOrDefaultAsync(f => f.Id == request.Id);

        if (video is null)
        {
            return Result.Fail(new Error($"Cannot find a Video with corresponding Id: {request.Id}"));
        }

        var videoDto = _mapper.Map<VideoDTO>(video);
        return Result.Ok(videoDto);
    }
}