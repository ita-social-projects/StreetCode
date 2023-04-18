using AutoMapper;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.DTO.Timeline;
using Streetcode.BLL.Util;
using Streetcode.DAL.Entities.Streetcode.Types;
using Streetcode.DAL.Entities.Timeline;

namespace Streetcode.BLL.Mapping.Timeline;

public class TimelineItemProfile : Profile
{
    public TimelineItemProfile()
    {
        CreateMap<TimelineItem, TimelineItemDTO>()
            .ForPath(dto => dto.DateString, conf => conf
                .MapFrom(t => DateToStringConverter.FromDateToString(t.Date, t.DateViewPattern)));
    }
}