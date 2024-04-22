using AutoMapper;
using Streetcode.BLL.DTO.Team;
using Streetcode.DAL.Entities.Team;

namespace Streetcode.BLL.Mapping.Team
{
    public class TeamProfile : Profile
    {
        public TeamProfile()
        {
            CreateMap<TeamMember, TeamMemberDTO>().ReverseMap();
            CreateMap<TeamMember, CreateTeamMemberDTO>().ReverseMap();
            CreateMap<TeamMember, UpdateTeamMemberDTO>().ReverseMap();
            CreateMap<TeamMemberCreateDTO, TeamMember>();
        }
    }
}
