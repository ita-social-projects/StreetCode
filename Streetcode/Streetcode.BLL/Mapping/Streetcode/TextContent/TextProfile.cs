using AutoMapper;
using Streetcode.BLL.DTO.Streetcode.TextContent.Text;
using Streetcode.DAL.Entities.Streetcode.TextContent;

namespace Streetcode.BLL.Mapping.Streetcode.TextContent;

public class TextProfile : Profile
{
    public TextProfile()
    {
        CreateMap<Text, TextDto>().ReverseMap();
        CreateMap<TextCreateDto, Text>().ReverseMap();
        CreateMap<TextUpdateDto, Text>().ReverseMap();
    }
}
