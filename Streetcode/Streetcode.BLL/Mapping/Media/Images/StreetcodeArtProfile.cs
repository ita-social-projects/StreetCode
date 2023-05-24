using AutoMapper;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.DTO.Streetcode.Update.TextContent;
using Streetcode.DAL.Entities.Streetcode;

namespace Streetcode.BLL.Mapping.Media.Images;

public class StreetcodeArtProfile : Profile
{
    public StreetcodeArtProfile()
    {
        CreateMap<StreetcodeArt, StreetcodeArtDTO>().ReverseMap();

        CreateMap<StreetcodeArtUpdateDTO, StreetcodeArt>().ReverseMap();
    }
}
