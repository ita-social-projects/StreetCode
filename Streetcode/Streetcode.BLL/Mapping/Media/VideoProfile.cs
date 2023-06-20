using AutoMapper;
using Streetcode.BLL.DTO.Media.Video;
using Streetcode.DAL.Entities.Media;

namespace Streetcode.BLL.Mapping.Media;

public class VideoProfile : Profile
{
    public VideoProfile()
    {
        CreateMap<Video, VideoDTO>();

        CreateMap<VideoCreateDTO, Video>().ReverseMap();

        CreateMap<Video, VideoUpdateDTO>().ReverseMap();
    }
}
