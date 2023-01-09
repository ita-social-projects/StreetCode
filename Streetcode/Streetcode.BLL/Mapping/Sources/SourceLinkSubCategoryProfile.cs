using AutoMapper;
using Streetcode.BLL.DTO.Sources;
using Streetcode.DAL.Entities.Sources;

namespace Streetcode.BLL.Mapping.Sources;

public class SourceLinkSubCategoryProfile : Profile
{
    public SourceLinkSubCategoryProfile()
    {
        CreateMap<SourceLinkSubCategory, SourceLinkSubCategoryDTO>()
            .ForMember(s => s.SourceLinks, c => c.MapFrom(b => b.SourceLinks))
            .ForMember(s => s.SourceLinkCategory, c => c.Ignore())
            .ReverseMap();
    }
}