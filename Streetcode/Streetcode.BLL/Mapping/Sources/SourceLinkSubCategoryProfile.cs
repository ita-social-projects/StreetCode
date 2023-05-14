using AutoMapper;
using Streetcode.BLL.DTO.Sources;
using Streetcode.DAL.Entities.Sources;

namespace Streetcode.BLL.Mapping.Sources;

public class SourceLinkSubCategoryProfile : Profile
{
    public SourceLinkSubCategoryProfile()
    {
        CreateMap<CategoryContentCreateDTO, StreetcodeCategoryContent>().ReverseMap();
    }
}
