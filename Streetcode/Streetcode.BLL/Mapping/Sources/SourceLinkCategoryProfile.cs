using AutoMapper;
using Streetcode.BLL.DTO.Sources;
using Streetcode.DAL.Entities.Sources;

namespace Streetcode.BLL.Mapping.Sources;

public class SourceLinkCategoryProfile : Profile
{
    public SourceLinkCategoryProfile()
    {
        CreateMap<SourceLinkCategory, SourceLinkCategoryDTO>()
            .ForMember(dto => dto.SubCategories, c => c.MapFrom(b => b.StreetcodeCategoryContents))
            .ForMember(dto => dto.Image, c => c.MapFrom(b => b.Image))
            .ForPath(dto => dto.Image!.Streetcodes, c => c.Ignore())
            .ReverseMap();
        CreateMap<SourceLinkCategory, CategoryWithNameDTO>().ReverseMap();
    }
}
