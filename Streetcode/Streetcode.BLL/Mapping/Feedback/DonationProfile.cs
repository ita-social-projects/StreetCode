using AutoMapper;
using Streetcode.BLL.DTO.Feedback;
using Streetcode.DAL.Entities.Feedback;

namespace Streetcode.BLL.Mapping.Feedback;

internal class DonationProfile : Profile
{
    public DonationProfile()
    {
        CreateMap<Donation, DonationDTO>().ReverseMap();
    }
}
