using AutoMapper;
using Streetcode.BLL.DTO.Media;
using Streetcode.BLL.DTO.Media.Create;
using Streetcode.BLL.DTO.Streetcode.Update.Media;
using Streetcode.DAL.Entities.Media;

namespace Streetcode.BLL.Mapping.Media;

public class VideoProfile : Profile
{
    public VideoProfile()
    {
        CreateMap<Video, VideoDTO>()
            .ForPath(dto => dto.Url.Title, conf => conf.MapFrom(ol => ol.Title))
            .ForPath(dto => dto.Url.Href, conf => conf.MapFrom(ol => ol.Url));

        CreateMap<VideoCreateDTO, Video>().ReverseMap();

        CreateMap<Video, VideoUpdateDTO>()
            .ForPath(dto => dto.Url.Title, conf => conf.MapFrom(ol => ol.Title))
            .ForPath(dto => dto.Url.Href, conf => conf.MapFrom(ol => ol.Url)).ReverseMap();
    }
}
