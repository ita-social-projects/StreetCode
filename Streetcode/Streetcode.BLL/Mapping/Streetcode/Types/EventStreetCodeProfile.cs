using AutoMapper;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.DTO.Streetcode.Create;
using Streetcode.BLL.DTO.Streetcode.Types;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Streetcode.Types;

namespace Streetcode.BLL.Mapping.Streetcode.Types;

public class EventStreetcodeProfile : Profile
{
    public EventStreetcodeProfile()
    {
        CreateMap<EventStreetcode, EventStreetcodeDto>()
            .IncludeBase<StreetcodeContent, StreetcodeDto>().ReverseMap();

        CreateMap<EventStreetcode, StreetcodeCreateDto>()
            .IncludeBase<StreetcodeContent, StreetcodeCreateDto>().ReverseMap();
  }
}
