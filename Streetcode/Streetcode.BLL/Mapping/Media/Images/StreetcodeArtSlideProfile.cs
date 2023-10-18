using AutoMapper;
using Streetcode.BLL.DTO.ArtGallery.ArtSlide;
using Streetcode.BLL.DTO.Media.Art;
using Streetcode.DAL.Entities.Streetcode;

namespace Streetcode.BLL.Mapping.Media.Images;

public class StreetcodeArtSlideProfile : Profile
{
    public StreetcodeArtSlideProfile()
    {
        // CreateMap<StreetcodeArtSlide, StreetcodeArtSlideDTO>().ReverseMap();
        CreateMap<StreetcodeArtSlide, ArtSlideDTO>().ReverseMap();
        CreateMap<StreetcodeArtSlideCreateUpdateDTO, StreetcodeArtSlide>().ReverseMap();
    }
}
