using AutoMapper;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.DTO.Streetcode.Types;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Streetcode.Types;

namespace Streetcode.BLL.Mapping.Streetcode.Types;

public class GeneralStreetcodeProfile : Profile
{
    public GeneralStreetcodeProfile()
    {
        CreateMap<EventStreetcode, GeneralStreetodeDTO>()
            .IncludeBase<StreetcodeContent, StreetcodeDTO>().ReverseMap();

        CreateMap<PersonStreetcode, GeneralStreetodeDTO>()
            .IncludeBase<StreetcodeContent, StreetcodeDTO>().ReverseMap();
    }
}
