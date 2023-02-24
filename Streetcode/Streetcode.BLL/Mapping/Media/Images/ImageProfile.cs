using AutoMapper;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.DAL.Entities.Media.Images;

namespace Streetcode.BLL.Mapping.Media.Images;

public class ImageProfile : Profile
{
    public ImageProfile()
    {
        CreateMap<Image, ImageDTO>()
            .ForPath(dto => dto.Url.Title, conf => conf.MapFrom(ol => ol.Title))
            .ForPath(dto => dto.Url.Href, conf => conf.MapFrom(ol => ol.Url))
            .ForPath(dto => dto.Url.Href, conf => conf.MapFrom(ol => ol.Url))
            .ReverseMap();
    }
}
