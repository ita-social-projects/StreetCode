using AutoMapper;
using Streetcode.BLL.DTO.Sources;
using Streetcode.DAL.Entities.Sources;

namespace Streetcode.BLL.Mapping.Sources;

public class SourceLinkSubCategoryProfile : Profile
{
    public SourceLinkSubCategoryProfile()
    {
        CreateMap<StreetcodeCategoryContent, SourceLinkSubCategoryDTO>()
            .ForMember(s => s.SourceLinkCategory, c => c.Ignore())
            .ReverseMap();
    }
}