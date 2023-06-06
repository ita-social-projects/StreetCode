using AutoMapper;
using Streetcode.BLL.DTO.Media.Video;
using Streetcode.DAL.Entities.Media;

namespace Streetcode.BLL.Mapping.Media;

public class VideoProfile : Profile
{
    public VideoProfile()
    {
        CreateMap<Video, VideoDTO>();
            /*.ForPath(dto => dto.Url.Title, conf => conf.MapFrom(ol => ol.Title))
            .ForPath(dto => dto.Url.Href, conf => conf.MapFrom(ol => ol.Url));*/

        CreateMap<VideoCreateDTO, Video>().ReverseMap();

        CreateMap<Video, VideoUpdateDTO>().ReverseMap();
            /*.ForPath(dto => dto.Url.Title, conf => conf.MapFrom(ol => ol.Title))
            .ForPath(dto => dto.Url.Href, conf => conf.MapFrom(ol => ol.Url)).ReverseMap();*/
    }
}
