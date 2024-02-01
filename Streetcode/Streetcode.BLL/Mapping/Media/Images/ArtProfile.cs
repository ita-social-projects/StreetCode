using AutoMapper;
using Streetcode.BLL.DTO.Media.Art;
using Streetcode.BLL.DTO.Media.Create;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Entities.Streetcode;

namespace Streetcode.BLL.Mapping.Media.Images;

public class ArtProfile : Profile
{
    public ArtProfile()
    {
        CreateMap<Art, ArtDTO>().ReverseMap();
        CreateMap<ArtCreateUpdateDTO, Art>()
            .ForMember(a => a.Id, conf => conf.Ignore())
            .ForMember(a => a.StreetcodeId, conf => conf.Ignore())
            .ForMember(a => a.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(a => a.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(a => a.ImageId, opt => opt.MapFrom(src => src.ImageId))
            .ForMember(a => a.Image, conf => conf.Ignore())
            .ForMember(a => a.Streetcode, conf => conf.Ignore())
            .ForMember(a => a.StreetcodeArts, conf => conf.Ignore())
            .ReverseMap();
    }
}
