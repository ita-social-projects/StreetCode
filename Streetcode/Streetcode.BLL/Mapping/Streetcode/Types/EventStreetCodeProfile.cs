using AutoMapper;
using Streetcode.BLL.DTO.Streetcode.Types;
using Streetcode.DAL.Entities.Streetcode.Types;

namespace Streetcode.BLL.Mapping.Streetcode.Types;

public class EventStreetCodeProfile : Profile
{
    public EventStreetCodeProfile()
    {
        CreateMap<EventStreetCode, EventStreetcodeDTO>().ReverseMap();
    }
}