using AutoMapper;
using Streetcode.BLL.DTO.Partners;
using Streetcode.DAL.Entities.Partners;

namespace Streetcode.BLL.Mapping.Partners;

public class PartnerSourceLinkProfile : Profile
{
    public PartnerSourceLinkProfile()
    {
        CreateMap<PartnerSourceLink, PartnerSourceLinkDTO>()
            .ForPath(dto => dto.TargetUrl.Title, conf => conf.MapFrom(ol => ol.Title))
            .ForPath(dto => dto.TargetUrl.Href, conf => conf.MapFrom(ol => ol.TargetUrl));
        CreateMap<PartnerSourceLink, CreatePartnerSourceLinkDTO>().ReverseMap();
    }
}
