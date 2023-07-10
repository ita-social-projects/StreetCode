using AutoMapper;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.DTO.Partners.Create;
using Streetcode.DAL.Entities.Partners;

namespace Streetcode.BLL.Mapping.Partners;

public class PartnerSourceLinkProfile : Profile
{
    public PartnerSourceLinkProfile()
    {
        CreateMap<PartnerSourceLink, PartnerSourceLinkDTO>()
            .ForPath(dto => dto.TargetUrl.Href, conf => conf.MapFrom(ol => ol.TargetUrl));
        CreateMap<PartnerSourceLink, CreatePartnerSourceLinkDTO>().ReverseMap();
    }
}
