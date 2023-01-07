using AutoMapper;
using Streetcode.BLL.DTO.Sources;
using Streetcode.DAL.Entities.Sources;

namespace Streetcode.BLL.Mapping.Sources;

public class SourceLinkSubCategoryProfile : Profile
{
    public SourceLinkSubCategoryProfile()
    {
        CreateMap<SourceLinkSubCategory, SourceLinkSubCategoryDTO>()
            .ForMember(d => d.SourceLinks, c => c.Ignore())
            .ReverseMap();
    }
}