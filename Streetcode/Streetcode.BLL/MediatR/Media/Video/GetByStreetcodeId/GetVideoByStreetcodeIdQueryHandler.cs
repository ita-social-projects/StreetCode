using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Media;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Media.Video.GetByStreetcodeId
{
    public class GetVideoByStreetcodeIdQueryHandler : IRequestHandler<GetVideoByStreetcodeIdQuery, Result<IEnumerable<VideoDTO>>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;

        public GetVideoByStreetcodeIdQueryHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<VideoDTO>>> Handle(GetVideoByStreetcodeIdQuery request, CancellationToken cancellationToken)
        {
            var video = await _repositoryWrapper.VideoRepository.GetAllAsync(video => video.StreetcodeId == request.streetcodeId);

            if (video is null)
            {
                return Result.Fail(new Error("Can`t find video with this StreetcodeId"));
            }

            var videoDto = _mapper.Map<IEnumerable<VideoDTO>>(video);
            return Result.Ok(videoDto);
        }
    }
}
