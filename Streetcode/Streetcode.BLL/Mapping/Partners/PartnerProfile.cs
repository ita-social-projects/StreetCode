using AutoMapper;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.DTO.Streetcode.Update;
using Streetcode.DAL.Entities.Partners;

namespace Streetcode.BLL.Mapping.Partners;

public class PartnerProfile : Profile
{
    public PartnerProfile()
    {
        CreateMap<Partner, PartnerDTO>()
            .ForPath(dto => dto.TargetUrl.Title, conf => conf.MapFrom(ol => ol.UrlTitle))
            .ForPath(dto => dto.TargetUrl.Href, conf => conf.MapFrom(ol => ol.TargetUrl));
        CreateMap<Partner, CreatePartnerDTO>().ReverseMap();
        CreateMap<Partner, PartnerShortDTO>().ReverseMap();
        CreateMap<StreetcodePartner, PartnersUpdateDTO>()
            .ForPath(dto => dto.PartnerId, conf => conf.MapFrom(ol => ol.PartnerId))
            .ForPath(dto => dto.StreetcodeId, conf => conf.MapFrom(ol => ol.StreetcodeId));
        CreateMap<PartnersUpdateDTO, StreetcodePartner>().ReverseMap();
    }
}