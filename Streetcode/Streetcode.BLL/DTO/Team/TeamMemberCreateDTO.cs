using Streetcode.BLL.DTO.Team.Abstractions;

namespace Streetcode.BLL.DTO.Team
{
    public class TeamMemberCreateDto : TeamMemberCreateUpdateDto
    {
        public List<TeamMemberLinkCreateDto>? TeamMemberLinks { get; set; } = new List<TeamMemberLinkCreateDto>();
    }
}
