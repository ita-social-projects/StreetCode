using AutoMapper;
using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.DAL.Entities.AdditionalContent;

namespace Streetcode.BLL.Mapping.AdditionalContent;

public class SubtitleProfile : Profile
{
   public SubtitleProfile()
   {
        CreateMap<Subtitle, SubtitleDTO>().ReverseMap();
   }
}
