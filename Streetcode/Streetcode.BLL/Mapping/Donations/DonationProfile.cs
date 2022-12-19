using AutoMapper;
using Streetcode.BLL.DTO.Donates;
using Streetcode.DAL.Entities.Donations;

namespace Streetcode.BLL.Mapping.Donations;

internal class DonationProfile : Profile
{
    public DonationProfile()
    {
        CreateMap<Donation, DonationDTO>().ReverseMap();
    }
}
