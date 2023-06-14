using AutoMapper;
using Streetcode.BLL.DTO.Timeline;
using Streetcode.BLL.DTO.Timeline.Update;
using Streetcode.DAL.Entities.Timeline;

namespace Streetcode.BLL.Mapping.Timeline;

public class TimelineItemProfile : Profile
{
    public TimelineItemProfile()
    {
        CreateMap<TimelineItem, TimelineItemDTO>().ReverseMap();
        CreateMap<TimelineItemUpdateDTO, TimelineItem>()
            .BeforeMap((src, dest) => dest.Streetcode = null)
            .ForMember(dest => dest.HistoricalContextTimelines, opt => opt.MapFrom(x => x.HistoricalContexts
            .Select(t => new HistoricalContextTimeline
            {
                TimelineId = x.Id,
                HistoricalContextId = t.Id,
                HistoricalContext = t.Id <= 0 ? new HistoricalContext { Id = t.Id, Title = t.Title } : null,
            })))
            .ReverseMap();

        CreateMap<TimelineItem, TimelineItemDTO>()
            .ForMember(dest => dest.HistoricalContexts, opt => opt.MapFrom(x => x.HistoricalContextTimelines
                .Select(x => new HistoricalContextDTO
                {
                    Id = x.HistoricalContextId,
                    Title = x.HistoricalContext.Title
                }).ToList()));
    }
}
