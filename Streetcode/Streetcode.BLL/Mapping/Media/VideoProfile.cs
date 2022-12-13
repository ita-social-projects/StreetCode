using AutoMapper;
using Streetcode.BLL.DTO.Media;
using Streetcode.DAL.Entities.Media;

namespace Streetcode.BLL.Mapping.Media;


public class VideoProfile : Profile
{
    public VideoProfile()
    {
        CreateMap<Video, VideoDTO>().ReverseMap();
    }
}
