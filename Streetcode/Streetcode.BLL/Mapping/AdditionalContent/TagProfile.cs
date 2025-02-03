using AutoMapper;
using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.DTO.AdditionalContent.Tag;
using Streetcode.DAL.Entities.AdditionalContent;

namespace Streetcode.BLL.Mapping.AdditionalContent;

public class TagProfile : Profile
{
    public TagProfile()
    {
        CreateMap<Tag, TagDto>().ReverseMap();
        CreateMap<Tag, UpdateTagDto>().ReverseMap();
        CreateMap<Tag, StreetcodeTagDto>().ReverseMap();
        CreateMap<StreetcodeTagIndex, StreetcodeTagDto>()
            .ForMember(x => x.Id, conf => conf.MapFrom(ti => ti.TagId))
            .ForMember(x => x.Title, conf => conf.MapFrom(ti => ti.Tag!.Title ?? ""));

        CreateMap<StreetcodeTagUpdateDto, StreetcodeTagIndex>()
            .ForMember(x => x.TagId, conf => conf.MapFrom(ti => ti.Id))
            .ForPath(sti => sti.Tag, conf => conf.MapFrom(stu => stu.Id <= 0 ? new Tag() { Id = stu.Id, Title = stu.Title } : null));

        CreateMap<StreetcodeTagUpdateDto, Tag>()
            .ForMember(t => t.Id, conf => conf.MapFrom(stu => stu.Id))
            .ForMember(t => t.Title, conf => conf.MapFrom(stu => stu.Title));

        CreateMap<StreetcodeTagDto, StreetcodeTagUpdateDto>().ReverseMap();
        CreateMap<Tag, StreetcodeTagUpdateDto>().ReverseMap();
    }
}
