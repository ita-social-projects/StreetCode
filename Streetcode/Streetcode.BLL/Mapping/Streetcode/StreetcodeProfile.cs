using AutoMapper;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.DTO.Streetcode.Create;
using Streetcode.BLL.DTO.Streetcode.Update;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Streetcode.Types;
using Streetcode.DAL.Enums;
using StringToDateTimeConverter = Streetcode.BLL.Mapping.Converters.StringToDateTimeConverter;

namespace Streetcode.BLL.Mapping.Streetcode;

public class StreetcodeProfile : Profile
{
    public StreetcodeProfile()
    {
        CreateMap<StreetcodeContent, StreetcodeDTO>()
            .ForMember(x => x.StreetcodeType, conf => conf.MapFrom(s => GetStreetcodeType(s)))
            .ReverseMap();
        CreateMap<StreetcodeContent, StreetcodeShortDTO>().ReverseMap();
        CreateMap<StreetcodeContent, StreetcodeMainPageDTO>()
             .ForPath(dto => dto.Text, conf => conf
                .MapFrom(e => e.Text!.Title))
             .ForPath(dto => dto.ImageId, conf => conf
                .MapFrom(e => e.Images.Select(i => i.Id).FirstOrDefault()));

        CreateMap<StreetcodeContent, StreetcodeFavouriteDTO>()
            .ForMember(x => x.Type, conf => conf.MapFrom(s => GetStreetcodeType(s)))
             .ForPath(dto => dto.ImageId, conf => conf
                .MapFrom(e => e.Images.Select(i => i.Id).FirstOrDefault()))
             .ReverseMap();

        CreateMap<StreetcodeCreateDTO, StreetcodeContent>()
                .ForMember(x => x.Arts, conf => conf.Ignore())
                .ForMember(x => x.StreetcodeArtSlides, conf => conf.Ignore())
                .ForMember(x => x.Tags, conf => conf.Ignore())
                .ForMember(x => x.Partners, conf => conf.Ignore())
                .ForMember(x => x.Toponyms, conf => conf.Ignore())
                .ForMember(x => x.TimelineItems, conf => conf.Ignore())
                .ForMember(x => x.Images, conf => conf.Ignore())
                .ForMember(x => x.StatisticRecords, conf => conf.Ignore())
                .ForMember(x => x.StreetcodeArtSlides, conf => conf.Ignore())
                .ReverseMap();

        CreateMap<StreetcodeUpdateDTO, StreetcodeContent>()
            .ForMember(x => x.Text, conf => conf.Ignore())
            .ForMember(x => x.Tags, conf => conf.Ignore())
            .ForMember(x => x.Partners, conf => conf.Ignore())
            .ForMember(x => x.Toponyms, conf => conf.Ignore())
            .ForMember(x => x.TimelineItems, conf => conf.Ignore())
            .ForMember(x => x.Images, conf => conf.Ignore())
            .ForMember(x => x.StreetcodeCategoryContents, conf => conf.Ignore())
            .ForMember(x => x.StatisticRecords, conf => conf.Ignore())
            .ForMember(x => x.StreetcodeArtSlides, conf => conf.Ignore())
            .ForMember(x => x.Facts, conf => conf.Ignore())
            .ForPath(x => x.TransactionLink!.Url, conf => conf.MapFrom(x => x.ARBlockUrl))
              .ReverseMap();

        CreateMap<StreetcodeUpdateDTO, PersonStreetcode>()
            .IncludeBase<StreetcodeUpdateDTO, StreetcodeContent>()
            .ReverseMap();

        CreateMap<StreetcodeUpdateDTO, EventStreetcode>()
            .IncludeBase<StreetcodeUpdateDTO, StreetcodeContent>()
            .ReverseMap();
    }

    private StreetcodeType GetStreetcodeType(StreetcodeContent streetcode)
    {
        if(streetcode is EventStreetcode)
        {
            return StreetcodeType.Event;
        }

        return StreetcodeType.Person;
    }
}
