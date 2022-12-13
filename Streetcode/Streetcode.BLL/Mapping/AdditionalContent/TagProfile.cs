using AutoMapper;
using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.DAL.Entities.AdditionalContent;

namespace Streetcode.BLL.Mapping.AdditionalContent;

public class TagProfile:Profile
{
    public TagProfile()
    {
        CreateMap<Tag, TagDTO>().ReverseMap();
    }
}