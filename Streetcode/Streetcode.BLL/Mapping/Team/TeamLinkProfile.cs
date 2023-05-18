using AutoMapper;
using Streetcode.BLL.DTO.Team;
using Streetcode.DAL.Entities.Team;

namespace Streetcode.BLL.Mapping.Team
{
    public class TeamLinkProfile : Profile
    {
        public TeamLinkProfile()
        {
            CreateMap<TeamMemberLink, TeamMemberLinkDTO>().ReverseMap();
        }
    }
}
