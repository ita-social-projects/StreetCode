using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.AdditionalContent.Subtitles;
using Streetcode.BLL.DTO.Media.Video;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Media.Video.GetByStreetcodeId;

public class GetVideoByStreetcodeIdHandler : IRequestHandler<GetVideoByStreetcodeIdQuery, Result<VideoDTO>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService _logger;

    public GetVideoByStreetcodeIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<VideoDTO>> Handle(GetVideoByStreetcodeIdQuery request, CancellationToken cancellationToken)
    {
        var video = await _repositoryWrapper.VideoRepository
            .GetFirstOrDefaultAsync(video => video.StreetcodeId == request.StreetcodeId);

        if (video is null)
        {
            string errorMsg = $"Cannot find any video by the streetcode id: {request.StreetcodeId}";
            _logger?.LogError("GetVideoByStreetcodeIdQuery handled with an error");
            _logger?.LogError(errorMsg);
            return Result.Fail(new Error(errorMsg));
        }

        var videoDto = _mapper.Map<VideoDTO>(video);
        _logger?.LogInformation($"GetVideoByStreetcodeIdQuery handled successfully");
        return Result.Ok(videoDto);
    }
}