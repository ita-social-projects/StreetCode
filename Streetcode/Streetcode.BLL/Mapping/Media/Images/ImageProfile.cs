using AutoMapper;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Entities.Streetcode;

namespace Streetcode.BLL.Mapping.Media.Images;

public class ImageProfile : Profile
{
    public ImageProfile()
    {
        CreateMap<Image, ImageDto>().ReverseMap();
        CreateMap<ImageDtoCreateEntity, Image>();

        CreateMap<ImageFileBaseCreateDto, Image>();

        CreateMap<ImageFileBaseUpdateDto, Image>();

        CreateMap<ImageUpdateDto, Image>();

        CreateMap<ImageUpdateDto, StreetcodeImage>()
            .ForMember(sim => sim.ImageId, opt => opt.MapFrom(siu => siu.Id))
            .ForMember(sim => sim.Image, opt => opt.MapFrom(src => null as Image))
            .ForMember(sim => sim.Streetcode, opt => opt.MapFrom(src => null as StreetcodeContent));
	}
}
