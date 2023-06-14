using AutoMapper;
using Streetcode.BLL.DTO.Timeline;
using Streetcode.BLL.DTO.Timeline.Update;
using Streetcode.DAL.Entities.AdditionalContent;
using Streetcode.DAL.Entities.Timeline;

namespace Streetcode.BLL.Mapping.Timeline;

public class HistoricalContextProfile : Profile
{
    public HistoricalContextProfile()
    {
        CreateMap<HistoricalContext, HistoricalContextDTO>().ReverseMap();
        CreateMap<HistoricalContext, HistoricalContextUpdateDTO>().ReverseMap();

        CreateMap<HistoricalContextUpdateDTO, HistoricalContextTimeline>()
          .ForMember(x => x.TimelineId, conf => conf.MapFrom(src => src.TimelineId))
          .ForMember(x => x.HistoricalContextId, conf => conf.MapFrom(src => src.Id))
          .ForMember(dest => dest.HistoricalContext, conf => conf.MapFrom(src => src.Id <= 0 ? new HistoricalContext() { Id = src.Id, Title = src.Title } : null));
    }
}
