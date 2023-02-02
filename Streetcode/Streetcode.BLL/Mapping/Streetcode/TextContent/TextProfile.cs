using AutoMapper;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.DAL.Entities.Streetcode.TextContent;

namespace Streetcode.BLL.Mapping.Streetcode.TextContent;

public class TextProfile : Profile
{
    public TextProfile()
    {
        CreateMap<Text, TextDTO>().ReverseMap();
    }
}