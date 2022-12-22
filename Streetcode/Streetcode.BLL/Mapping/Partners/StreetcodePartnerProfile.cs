using AutoMapper;
using Streetcode.BLL.DTO.Partners;
using Streetcode.DAL.Entities.Partners;

namespace Streetcode.BLL.Mapping.Partners;

public class StreetcodePartnerProfile : Profile
{
    public StreetcodePartnerProfile()
    {
        CreateMap<StreetcodePartner, StreetcodePartnerDTO>().ReverseMap();
    }
}
