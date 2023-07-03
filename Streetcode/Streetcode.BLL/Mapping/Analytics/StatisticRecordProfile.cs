using AutoMapper;
using Streetcode.BLL.DTO.Analytics;
using Streetcode.DAL.Entities.Analytics;

namespace Streetcode.BLL.Mapping.Analytics
{
    public class StatisticRecordProfile : Profile
    {
        public StatisticRecordProfile()
        {
            CreateMap<StatisticRecord, StatisticRecordDTO>().ReverseMap();
        }
    }
}
