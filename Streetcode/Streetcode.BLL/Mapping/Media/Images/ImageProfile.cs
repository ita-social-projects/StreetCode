using AutoMapper;
using FluentResults;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Entities.Streetcode;

namespace Streetcode.BLL.Mapping.Media.Images;

public class ImageProfile : Profile
{
    public ImageProfile()
    {
        CreateMap<Image, ImageDTO>()
            .ForMember(dest => dest.MimeType, opt => opt.MapFrom(src => src.MimeType));

        CreateMap<ImageFileBaseCreateDTO, Image>()
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.MimeType, opt => opt.MapFrom(src => src.MimeType));

        CreateMap<ImageFileBaseUpdateDTO, Image>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.MimeType, opt => opt.MapFrom(src => src.MimeType));

        CreateMap<ImageDTO, Image>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Alt))
            .ForMember(dest => dest.MimeType, opt => opt.MapFrom(src => src.MimeType))
            .ForMember(dest => dest.BlobName, opt => opt.MapFrom(src => src.BlobName))
            .ForMember(dest => dest.Alt, opt => opt.MapFrom(src => src.Alt));

        CreateMap<StreetcodeImageUpdateDTO, Image>();

        CreateMap<StreetcodeImageUpdateDTO, StreetcodeImage>()
            .ForMember(sim => sim.ImageId, opt => opt.MapFrom(siu => siu.Id))
            .ForMember(sim => sim.StreetcodeId, opt => opt.MapFrom(siu => siu.StreetcodeId))
            .ForMember(sim => sim.Image, opt => opt.MapFrom(src => null as Image))
            .ForMember(sim => sim.Streetcode, opt => opt.MapFrom(src => null as StreetcodeContent));
	}
}
