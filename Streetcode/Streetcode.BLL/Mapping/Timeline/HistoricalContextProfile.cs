using AutoMapper;
using Streetcode.BLL.DTO.Timeline;
using Streetcode.DAL.Entities.Timeline;

namespace Streetcode.BLL.Mapping.Timeline;

public class HistoricalContextProfile : Profile
{
    public HistoricalContextProfile()
    {
        CreateMap<HistoricalContext, HistoricalContextDTO>().ReverseMap();
    }
}