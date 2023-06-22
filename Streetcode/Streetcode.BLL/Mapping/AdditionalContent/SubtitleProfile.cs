using AutoMapper;
using Streetcode.BLL.DTO.AdditionalContent.Subtitles;
using Streetcode.DAL.Entities.AdditionalContent;

namespace Streetcode.BLL.Mapping.AdditionalContent;

public class SubtitleProfile : Profile
{
   public SubtitleProfile()
   {
        CreateMap<Subtitle, SubtitleDTO>().ReverseMap();
        CreateMap<SubtitleCreateDTO, Subtitle>().ReverseMap();
        CreateMap<SubtitleUpdateDTO, Subtitle>().ReverseMap();
  }
}
