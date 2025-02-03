using AutoMapper;
using Streetcode.BLL.DTO.ArtGallery.ArtSlide;
using Streetcode.BLL.DTO.Media.Art;
using Streetcode.DAL.Entities.Streetcode;

namespace Streetcode.BLL.Mapping.Media.Images;

public class StreetcodeArtSlideProfile : Profile
{
    public StreetcodeArtSlideProfile()
    {
        CreateMap<StreetcodeArtSlide, ArtSlideDto>().ReverseMap();

        CreateMap<StreetcodeArtSlideCreateUpdateDto, StreetcodeArtSlide>()
            .ForMember(s => s.Id, conf => conf.Ignore())
            .ForMember(s => s.StreetcodeId, conf => conf.Ignore())
            .ForMember(s => s.Index, opt => opt.MapFrom(src => src.Index))
            .ForMember(s => s.Template, opt => opt.MapFrom(src => src.Template))
            .ForMember(s => s.StreetcodeArts, conf => conf.Ignore())
            .ForMember(s => s.Streetcode, conf => conf.Ignore())
            .ReverseMap();

        CreateMap<StreetcodeArtSlideDto, StreetcodeArtSlide>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.SlideId))
            .ReverseMap();
    }
}
