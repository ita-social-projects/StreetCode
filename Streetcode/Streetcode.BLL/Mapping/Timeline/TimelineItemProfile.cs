using AutoMapper;
using Streetcode.BLL.DTO.Streetcode.Update.TextContent;
using Streetcode.BLL.DTO.Timeline;
using Streetcode.DAL.Entities.Timeline;

namespace Streetcode.BLL.Mapping.Timeline;

public class TimelineItemProfile : Profile
{
    public TimelineItemProfile()
    {
        CreateMap<TimelineItem, TimelineItemDTO>().ReverseMap();
        CreateMap<TimelineItemUpdateDTO, TimelineItem>()
          .BeforeMap((src, dest) => dest.Streetcode = null)
          .ReverseMap();
    }
}
