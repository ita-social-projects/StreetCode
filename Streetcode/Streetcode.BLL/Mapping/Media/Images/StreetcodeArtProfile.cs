using AutoMapper;
using Streetcode.BLL.DTO.Media.Art;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Entities.Streetcode;

namespace Streetcode.BLL.Mapping.Media.Images;

public class StreetcodeArtProfile : Profile
{
    public StreetcodeArtProfile()
    {
        CreateMap<StreetcodeArt, StreetcodeArtDTO>().ReverseMap();
        CreateMap<StreetcodeArtCreateUpdateDTO, StreetcodeArt>().ReverseMap();
    }
}
