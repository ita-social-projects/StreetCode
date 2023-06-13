using AutoMapper;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.DTO.Streetcode.Create;
using Streetcode.BLL.DTO.Streetcode.Update;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Streetcode.Types;

namespace Streetcode.BLL.Mapping.Streetcode;

public class StreetcodeProfile : Profile
{
    public StreetcodeProfile()
    {
        CreateMap<StreetcodeContent, StreetcodeDTO>().ReverseMap();
        CreateMap<StreetcodeContent, StreetcodeShortDTO>().ReverseMap();
        CreateMap<StreetcodeContent, StreetcodeMainPageDTO>()
             .ForPath(dto => dto.Text, conf => conf
                .MapFrom(e => e.Text.Title))
            .ForPath(dto => dto.ImageId, conf => conf
                .MapFrom(e => e.Images.Select(i => i.Id).LastOrDefault()));

        CreateMap<StreetcodeCreateDTO, StreetcodeContent>()
          .ForMember(x => x.Tags, conf => conf.Ignore())
          .ForMember(x => x.Partners, conf => conf.Ignore())
          .ForMember(x => x.Toponyms, conf => conf.Ignore())
          .ForMember(x => x.TimelineItems, conf => conf.Ignore())
          .ForMember(x => x.Images, conf => conf.Ignore())
          .ForMember(x => x.StatisticRecords, conf => conf.Ignore())
          .ForMember(x => x.StreetcodeArts, conf => conf.Ignore())
          .ReverseMap();

        /*CreateMap<StreetcodeContent, StreetcodeUpdateDTO>()
			.ForMember(su => su.StreetcodeToponym, conf => conf
				.MapFrom(s => s.Toponyms.Select(t =>
					new StreetcodeToponymUpdateDTO { StreetcodeId = s.Id, ToponymId = t.Id })));*/

        CreateMap<StreetcodeUpdateDTO, StreetcodeContent>()
        	    .ForMember(x => x.TimelineItems, conf => conf.Ignore())
        	    .ForMember(x => x.Partners, conf => conf.Ignore())
                .ForMember(x => x.StreetcodeArts, conf => conf.Ignore())
                .ForMember(x => x.StatisticRecords, conf => conf.Ignore())
                .ForMember(x => x.StreetcodeCategoryContents, conf => conf.Ignore())
        	    .ReverseMap();

        CreateMap<StreetcodeUpdateDTO, PersonStreetcode>()
          .ForMember(x => x.Tags, conf => conf.Ignore())
          .ForMember(x => x.Partners, conf => conf.Ignore())
          .ForMember(x => x.Toponyms, conf => conf.Ignore())
          .ForMember(x => x.TimelineItems, conf => conf.Ignore())
          .ForMember(x => x.Images, conf => conf.Ignore())
          .ForMember(x => x.StatisticRecords, conf => conf.Ignore())
          .ForMember(x => x.StreetcodeArts, conf => conf.Ignore())
        .ReverseMap();
    }
}
