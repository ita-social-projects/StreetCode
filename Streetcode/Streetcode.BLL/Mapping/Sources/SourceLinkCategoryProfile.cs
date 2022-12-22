using AutoMapper;
using Streetcode.BLL.DTO.Sources;
using Streetcode.DAL.Entities.Sources;

namespace Streetcode.BLL.Mapping.Sources;

public class SourceLinkCategoryProfile : Profile
{
    public SourceLinkCategoryProfile()
    {
        CreateMap<SourceLinkCategory, SourceLinkCategoryDTO>().ReverseMap();
    }
}
