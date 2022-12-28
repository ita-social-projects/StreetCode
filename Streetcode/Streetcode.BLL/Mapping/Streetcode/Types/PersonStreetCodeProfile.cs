using AutoMapper;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.DTO.Streetcode.Types;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Streetcode.Types;

namespace Streetcode.BLL.Mapping.Streetcode.Types;

public class PersonStreetcodeProfile : Profile
{
    public PersonStreetcodeProfile()
    {
        CreateMap<PersonStreetcode, PersonStreetcodeDTO>().IncludeBase<StreetcodeContent, StreetcodeDTO>().ReverseMap();
    }
}