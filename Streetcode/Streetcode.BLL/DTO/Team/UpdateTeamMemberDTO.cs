using Streetcode.BLL.DTO.Team.Abstractions;

namespace Streetcode.BLL.DTO.Team
{
    public class UpdateTeamMemberDTO : TeamMemberCreateUpdateDTO
    {
        public int Id { get; set; }
        public List<TeamMemberLinkDTO>? TeamMemberLinks { get; set; }
    }
}
