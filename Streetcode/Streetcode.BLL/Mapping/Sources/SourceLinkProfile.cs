using AutoMapper;
using Streetcode.BLL.DTO.Sources;
using Streetcode.DAL.Entities.Sources;

namespace Streetcode.BLL.Mapping.Sources;

public class SourceLinkProfile : Profile
{
    public SourceLinkProfile()
    {
        CreateMap<SourceLink, SourceLinkDTO>().ReverseMap();
    }
}


