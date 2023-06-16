using AutoMapper;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.DAL.Entities.Media.Images;

namespace Streetcode.BLL.Mapping.Media.Images;

public class ImageProfile : Profile
{
    public ImageProfile()
    {
        CreateMap<Image, ImageDTO>()
            .ForMember(dest => dest.MimeType, opt => opt.MapFrom(src => src.MimeType));

        CreateMap<ImageFileBaseCreateDTO, Image>()
            .ForMember(dest => dest.MimeType, opt => opt.MapFrom(src => src.MimeType));

        CreateMap<ImageFileBaseUpdateDTO, Image>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.MimeType, opt => opt.MapFrom(src => src.MimeType));

        CreateMap<ImageDTO, Image>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.MimeType, opt => opt.MapFrom(src => src.MimeType))
            .ForMember(dest => dest.BlobName, opt => opt.MapFrom(src => src.BlobName));
    }
}
