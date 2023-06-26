using AutoMapper;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.DAL.Entities.Streetcode.TextContent;

namespace Streetcode.BLL.Mapping.Streetcode.TextContent;

public class RelatedTermProfile : Profile
{
    public RelatedTermProfile()
    {
        CreateMap<RelatedTerm, RelatedTermDTO>().ReverseMap();
     }
}
