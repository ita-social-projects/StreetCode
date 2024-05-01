using Streetcode.BLL.DTO.Media.Images;

namespace Streetcode.BLL.DTO.Team
{
    public class TeamMemberCreateDTO
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool? IsMain { get; set; }
        public int? ImageId { get; set; }
        public List<TeamMemberLinkCreateDTO>? TeamMemberLinks { get; set; } = new List<TeamMemberLinkCreateDTO>();
        public List<PositionDTO>? Positions { get; set; } = new List<PositionDTO>();
    }
}
