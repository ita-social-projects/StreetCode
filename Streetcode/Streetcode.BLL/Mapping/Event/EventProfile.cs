using AutoMapper;
using Streetcode.BLL.DTO.Event;
using Streetcode.BLL.DTO.Event.CreateUpdate;
using Streetcode.DAL.Entities.Event;

namespace Streetcode.BLL.Mapping.Event
{
    public class EventProfile : Profile
    {
        public EventProfile()
        {
            CreateMap<DAL.Entities.Event.Event, EventDto>().ReverseMap();
            CreateMap<DAL.Entities.Event.Event, EventShortDto>().ReverseMap();

            CreateMap<HistoricalEvent, HistoricalEventDto>()
                .ReverseMap();

            CreateMap<CustomEvent, CustomEventDto>().ReverseMap()
                .ReverseMap();

            CreateMap<DAL.Entities.Event.Event, CreateUpdateEventDto>().ReverseMap();

            CreateMap<UpdateEventDto, DAL.Entities.Event.Event>()
                .ForMember(dest => dest.EventType, opt => opt.MapFrom(src => src.EventType.ToString()))
                .Include<UpdateEventDto, HistoricalEvent>()
                .Include<UpdateEventDto, CustomEvent>();

            CreateMap<UpdateEventDto, CustomEvent>()
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location))
                .ForMember(dest => dest.Organizer, opt => opt.MapFrom(src => src.Organizer));

            CreateMap<UpdateEventDto, HistoricalEvent>()
                .ForMember(dest => dest.TimelineItemId, opt => opt.MapFrom(src => src.TimelineItemId));
        }
    }
}
