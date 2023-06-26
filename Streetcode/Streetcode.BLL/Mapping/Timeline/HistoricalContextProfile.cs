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
        CreateMap<HistoricalContext, HistoricalContextCreateUpdateDTO>().ReverseMap();
    }
}
