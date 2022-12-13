using AutoMapper;
using Streetcode.BLL.DTO.Streetcode.Types;
using Streetcode.DAL.Entities.Streetcode.Types;

namespace Streetcode.BLL.Mapping.Streetcode.Types;

public class PersonStreetCodeProfile : Profile
{
    public PersonStreetCodeProfile()
    {
        CreateMap<PersonStreetCode, PersonStreetcodeDTO>().ReverseMap();
    }
}