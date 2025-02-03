using AutoMapper;
using Streetcode.BLL.DTO.Media.Video;
using Streetcode.DAL.Entities.Media;

namespace Streetcode.BLL.Mapping.Media;

public class VideoProfile : Profile
{
    public VideoProfile()
    {
        CreateMap<Video, VideoDto>();

        CreateMap<VideoCreateDto, Video>().ReverseMap();

        CreateMap<Video, VideoUpdateDto>().ReverseMap();
    }
}
