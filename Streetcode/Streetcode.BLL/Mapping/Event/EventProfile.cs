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
        }
    }
}
