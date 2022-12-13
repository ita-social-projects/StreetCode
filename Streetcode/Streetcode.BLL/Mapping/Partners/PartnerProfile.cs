using AutoMapper;
using Streetcode.BLL.DTO.Partners;
using Streetcode.DAL.Entities.Partners;

namespace Streetcode.BLL.Mapping.Partners;


public class PartnerProfile : Profile
{
    public PartnerProfile()
    {
        CreateMap<Partner, PartnerDTO>().ReverseMap();
    }
}