using AutoMapper;
using Streetcode.BLL.DTO.Analytics;
using Streetcode.BLL.DTO.Analytics.Update;
using Streetcode.DAL.Entities.Analytics;

namespace Streetcode.BLL.Mapping.Analytics
{
    public class StatisticRecordProfile : Profile
    {
        public StatisticRecordProfile()
        {
            CreateMap<StatisticRecord, StatisticRecordDTO>().ReverseMap();
            CreateMap<StatisticRecord, StatisticRecordUpdateDTO>().ReverseMap();
        }
    }
}
