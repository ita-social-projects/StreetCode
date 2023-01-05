using AutoMapper;
using Streetcode.BLL.DTO.Partners;
using Streetcode.DAL.Entities.Partners;

namespace Streetcode.BLL.Mapping.Partners;

public class PartnerProfile : Profile
{
    public PartnerProfile()
    {
        CreateMap<Partner, PartnerDTO>()
            .ForPath(dto => dto.TargetUrl.Title, conf => conf.MapFrom(ol => ol.Title))
            .ForPath(dto => dto.TargetUrl.Href, conf => conf.MapFrom(ol => ol.TargetUrl));
    }
}