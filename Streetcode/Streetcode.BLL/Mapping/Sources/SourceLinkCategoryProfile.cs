using AutoMapper;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.DTO.Sources;
using Streetcode.DAL.Entities.Sources;

namespace Streetcode.BLL.Mapping.Sources;

public class SourceLinkCategoryProfile : Profile
{
    public SourceLinkCategoryProfile()
    {
        CreateMap<SourceLinkCategory, SourceLinkCategoryDTO>()
            .ForMember(dto => dto.Image, c => c.MapFrom(b => b.Image))
            .ReverseMap();
        CreateMap<SourceLinkCategory, CategoryWithNameDTO>().ReverseMap();
        CreateMap<SourceLinkCategory, ImageDTO>()
            .ForMember(dest => dest.MimeType, opt => opt.MapFrom(src => src.Image.MimeType))
            .ForMember(dest => dest.BlobName, opt => opt.MapFrom(src => src.Image.BlobName));
        CreateMap<SourceLinkCategoryDTO, SourceLinkCategory>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(dto => dto.Title))
            .ForMember(dest => dest.Streetcodes, opt => opt.Ignore())
            .ForMember(dest => dest.StreetcodeCategoryContents, opt => opt.Ignore())
            .ForPath(dest => dest.Image!.Streetcodes, c => c.Ignore())
            .ForMember(dest => dest.ImageId, opt => opt.MapFrom(dto => dto.ImageId));
    }
}
