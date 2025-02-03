using AutoMapper;
using Streetcode.BLL.DTO.Analytics;
using Streetcode.BLL.DTO.Analytics.Update;
using Streetcode.DAL.Entities.AdditionalContent.Coordinates.Types;
using Streetcode.DAL.Entities.Analytics;

namespace Streetcode.BLL.Mapping.Analytics
{
    public class StatisticRecordProfile : Profile
    {
        public StatisticRecordProfile()
        {
            CreateMap<StatisticRecord, StatisticRecordDto>().ReverseMap();
            CreateMap<StatisticRecord, StatisticRecordResponseDto>().ReverseMap();

            CreateMap<StatisticRecordUpdateDto, StreetcodeCoordinate>()
                .ForMember(sc => sc.Id, conf => conf.MapFrom(sru => sru.StreetcodeCoordinate.Id))
                .ForMember(sc => sc.Latitude, conf => conf.MapFrom(sru => sru.StreetcodeCoordinate.Latitude))
                .ForMember(sc => sc.Longtitude, conf => conf.MapFrom(sru => sru.StreetcodeCoordinate.Longtitude))
                .ForMember(sc => sc.StatisticRecord, conf => conf.MapFrom(sru =>
                new StatisticRecord()
                {
                    Id = sru.Id,
                    QrId = sru.QrId,
                    Count = sru.Count,
                    Address = sru.Address,
                    StreetcodeId = sru.StreetcodeId,
                    StreetcodeCoordinateId = sru.StreetcodeCoordinate.Id,
                }));
        }
    }
}
