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
            CreateMap<DAL.Entities.Event.Event, EventDTO>().ReverseMap();
            CreateMap<DAL.Entities.Event.Event, EventShortDTO>().ReverseMap();

            CreateMap<HistoricalEvent, HistoricalEventDTO>()
                .ReverseMap();

            CreateMap<CustomEvent, CustomEventDTO>().ReverseMap()
                .ReverseMap();

            CreateMap<DAL.Entities.Event.Event, CreateUpdateEventDTO>().ReverseMap();

            CreateMap<UpdateEventDTO, DAL.Entities.Event.Event>()
                .ForMember(dest => dest.EventType, opt => opt.MapFrom(src => src.EventType.ToString()))
                .Include<UpdateEventDTO, HistoricalEvent>()
                .Include<UpdateEventDTO, CustomEvent>();

            CreateMap<UpdateEventDTO, CustomEvent>()
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location))
                .ForMember(dest => dest.Organizer, opt => opt.MapFrom(src => src.Organizer));

            CreateMap<UpdateEventDTO, HistoricalEvent>()
                .ForMember(dest => dest.TimelineItemId, opt => opt.MapFrom(src => src.TimelineItemId));
        }
    }
}
