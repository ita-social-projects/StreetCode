using Streetcode.BLL.DTO.Team.Abstractions;

namespace Streetcode.BLL.DTO.Team
{
    public class TeamMemberCreateDTO : TeamMemberCreateUpdateDTO
    {
        public List<TeamMemberLinkCreateDTO>? TeamMemberLinks { get; set; } = new List<TeamMemberLinkCreateDTO>();
    }
}
