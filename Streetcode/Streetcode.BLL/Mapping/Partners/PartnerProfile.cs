using AutoMapper;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.DTO.Partners.Update;
using Streetcode.DAL.Entities.Partners;

namespace Streetcode.BLL.Mapping.Partners;

public class PartnerProfile : Profile
{
    public PartnerProfile()
    {
        CreateMap<Partner, PartnerDTO>()
            .ForPath(dto => dto.TargetUrl!.Title, conf => conf.MapFrom(ol => ol.UrlTitle))
            .ForPath(dto => dto.TargetUrl!.Href, conf => conf.MapFrom(ol => ol.TargetUrl));
        CreateMap<Partner, CreatePartnerDTO>().ReverseMap();
        CreateMap<UpdatePartnerDTO, Partner>();
        CreateMap<Partner, PartnerShortDTO>().ReverseMap();
        CreateMap<PartnersUpdateDTO, StreetcodePartner>()
          .ReverseMap();
    }
}
