using AutoMapper;
using Streetcode.BLL.DTO.Media.Art;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Entities.Streetcode;

namespace Streetcode.BLL.Mapping.Media.Images;

public class StreetcodeArtProfile : Profile
{
    public StreetcodeArtProfile()
    {
        CreateMap<StreetcodeArt, StreetcodeArtDto>().ReverseMap();
        CreateMap<StreetcodeArtCreateUpdateDto, StreetcodeArt>()
            .ForMember(a => a.Id, conf => conf.Ignore())
            .ForMember(a => a.Index, conf => conf.MapFrom(src => src.Index))
            .ForMember(a => a.ArtId, conf => conf.Ignore())
            .ForMember(a => a.StreetcodeId, conf => conf.Ignore())
            .ForMember(a => a.StreetcodeArtSlideId, conf => conf.Ignore())
            .ForMember(a => a.Art, conf => conf.Ignore())
            .ForMember(a => a.StreetcodeArtSlide, conf => conf.Ignore())
            .ForMember(a => a.Streetcode, conf => conf.Ignore())
            .ReverseMap();

        CreateMap<Art, StreetcodeArt>()
            .ForMember(a => a.Id, opt => opt.Ignore())
            .ForMember(a => a.Index, opt => opt.Ignore())
            .ForMember(a => a.StreetcodeArtSlideId, opt => opt.Ignore())
            .ForMember(a => a.StreetcodeId, opt => opt.Ignore())
            .ForMember(a => a.Art, opt => opt.Ignore())
            .ForMember(a => a.StreetcodeArtSlide, opt => opt.Ignore())
            .ForMember(a => a.Streetcode, opt => opt.Ignore())
            .ForMember(a => a.ArtId, opt => opt.MapFrom(src => src.Id));
    }
}
