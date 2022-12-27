using AutoMapper;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.DTO.Streetcode.Types;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Streetcode.Types;

namespace Streetcode.BLL.Mapping.Streetcode;

public class PersonStreetcodeProfile : Profile
{
    public PersonStreetcodeProfile()
    {
        CreateMap<PersonStreetcode, PersonStreetcodeDTO>()
            .ForMember(dto => dto.Streetcode, conf => conf.MapFrom((order, orderDto, i, context) =>
            {
                var dd = context.Mapper.Map<StreetcodeContent, StreetcodeDTO>(new StreetcodeContent()
                {
                    Id = order.Id,
                    Index = order.Index,
                    Toponyms = order.Toponyms,
                    Coordinate = order.Coordinate,
                    Images = order.Images,
                    EventStartOrPersonBirthDate = order.EventStartOrPersonBirthDate,
                    EventEndOrPersonDeathDate = order.EventEndOrPersonDeathDate,
                    ViewCount = order.ViewCount,
                    CreatedAt = order.CreatedAt,
                    UpdatedAt = order.UpdatedAt,
                    Tags = order.Tags,
                    Teaser = order.Teaser,
                    Audio = order.Audio,
                    TransactionLink = order.TransactionLink,
                    Text = order.Text,
                    Videos = order.Videos,
                    Facts = order.Facts,
                    TimelineItems = order.TimelineItems,
                    SourceLinks = order.SourceLinks,
                    Arts = order.Arts,
                    Subtitles = order.Subtitles,
                    StreetcodePartners = order.StreetcodePartners,
                    Observers = order.Observers,
                    Targets = order.Targets,
                });
                return dd;
            }));
    }
}