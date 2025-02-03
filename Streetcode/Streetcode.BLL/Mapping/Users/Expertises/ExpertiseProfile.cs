using AutoMapper;
using Streetcode.BLL.DTO.Users.Expertise;
using Streetcode.DAL.Entities.Users.Expertise;

namespace Streetcode.BLL.Mapping.Users.Expertises;

public class ExpertiseProfile : Profile
{
    public ExpertiseProfile()
    {
        CreateMap<ExpertiseDTO, Expertise>().ForMember(x => x.Users, conf => conf.Ignore()).ReverseMap();
    }
}