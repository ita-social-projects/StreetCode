using AutoMapper;
using Streetcode.BLL.DTO.AdditionalContent.Subtitles;
using Streetcode.DAL.Entities.AdditionalContent;

namespace Streetcode.BLL.Mapping.AdditionalContent;

public class SubtitleProfile : Profile
{
   public SubtitleProfile()
   {
        CreateMap<Subtitle, SubtitleDTO>()
            .ForPath(dto => dto.Url.Href, conf => conf.MapFrom(ol => ol.Url))
            .ForPath(dto => dto.SubtitleStatus, conf => conf.MapFrom(ol => ol.Status))
            .ForPath(dto => dto.Url.Title, conf => conf.MapFrom(ol => ol.Title));
   }
}
