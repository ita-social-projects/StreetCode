using AutoMapper;
using Streetcode.BLL.DTO.Media;
using Streetcode.DAL.Entities.Media;

namespace Streetcode.BLL.Mapping.Media;

public class AudioProfile : Profile
{
    public AudioProfile()
    {
        CreateMap<Audio, AudioDTO>()
            .ForPath(dto => dto.Url.Title, conf => conf.MapFrom(ol => ol.Title))
            .ForPath(dto => dto.Url.Href, conf => conf.MapFrom(ol => ol.Url));
    }
}