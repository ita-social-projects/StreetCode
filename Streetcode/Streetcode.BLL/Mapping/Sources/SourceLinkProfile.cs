using AutoMapper;
using Streetcode.BLL.DTO.Sources;
using Streetcode.DAL.Entities.Sources;

namespace Streetcode.BLL.Mapping.Sources;

public class SourceLinkProfile : Profile
{
    public SourceLinkProfile()
    {
        CreateMap<SourceLink, SourceLinkDTO>()
            .ForPath(dto => dto.Url.Title, conf => conf.MapFrom(ol => ol.Title))
            .ForPath(dto => dto.Url.Href, conf => conf.MapFrom(ol => ol.Url))
            .ForMember(d => d.SubCategories, conf => conf.MapFrom(ol => ol.SubCategories))
            .ForMember(d => d.Streetcode, conf => conf.MapFrom(ol => ol.Streetcode));
    }
}