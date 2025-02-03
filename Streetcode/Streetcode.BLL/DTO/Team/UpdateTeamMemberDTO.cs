using Streetcode.BLL.DTO.Team.Abstractions;

namespace Streetcode.BLL.DTO.Team
{
    public class UpdateTeamMemberDto : TeamMemberCreateUpdateDto
    {
        public int Id { get; set; }
        public List<TeamMemberLinkDto>? TeamMemberLinks { get; set; }
    }
}
