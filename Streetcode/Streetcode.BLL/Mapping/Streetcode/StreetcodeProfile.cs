using AutoMapper;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.DTO.Streetcode.Create;
using Streetcode.BLL.DTO.Streetcode.Update;
using Streetcode.BLL.Util.MappingResolvers;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Streetcode.Types;
using Streetcode.DAL.Enums;
using Streetcode.DAL.Repositories.Interfaces.Base;
using StringToDateTimeConverter = Streetcode.BLL.Mapping.Converters.StringToDateTimeConverter;

namespace Streetcode.BLL.Mapping.Streetcode;

public class StreetcodeProfile : Profile
{
    public StreetcodeProfile()
    {
        CreateMap<StreetcodeContent, StreetcodeDto>()
            .ForMember(x => x.StreetcodeType, conf => conf.MapFrom(s => GetStreetcodeType(s)))
            .ForMember(x => x.CreatedBy, conf => conf.MapFrom<UserNameResolver>())
            .ReverseMap();
        CreateMap<StreetcodeContent, StreetcodeShortDto>().ReverseMap();
        CreateMap<StreetcodeContent, StreetcodeMainPageDto>()
             .ForPath(dto => dto.Text, conf => conf
                .MapFrom(e => e.Text!.Title))
             .ForPath(dto => dto.ImageId, conf => conf
                .MapFrom(e => e.Images.Select(i => i.Id).FirstOrDefault()));

        CreateMap<StreetcodeCreateDto, StreetcodeContent>()
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

        CreateMap<StreetcodeUpdateDto, StreetcodeContent>()
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

        CreateMap<StreetcodeUpdateDto, PersonStreetcode>()
            .IncludeBase<StreetcodeUpdateDto, StreetcodeContent>()
            .ReverseMap();

        CreateMap<StreetcodeUpdateDto, EventStreetcode>()
            .IncludeBase<StreetcodeUpdateDto, StreetcodeContent>()
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
